using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;
using BranchAppService = TheBeautyHubCore.Services.BranchService;

namespace TheBeautyHub.Tests.Services;

public class BranchServiceTests
{
    private readonly Mock<IBranchRepository> _branches = new();
    private readonly Mock<IStaffRepository> _staff = new();
    private readonly BranchAppService _sut;

    public BranchServiceTests()
    {
        _sut = new BranchAppService(_branches.Object, _staff.Object);
    }

    [Fact]
    public async Task Create_accepts_android_active_status_and_empty_services()
    {
        _branches.Setup(r => r.InsertAsync(It.IsAny<Branch>()))
            .ReturnsAsync((Branch b) => { b.BranchId = TestIds.Branch; return b; });
        _branches.Setup(r => r.ReplaceServicesAsync(TestIds.Branch, It.IsAny<IEnumerable<Guid>>()))
            .Returns(Task.CompletedTask);

        var saved = await _sut.CreateBranchAsync(Fixtures.BranchCreate());

        Assert.True(saved.Saved);
        _branches.Verify(r => r.InsertAsync(It.Is<Branch>(b => b.Status == RecordStatus.Active.ToApiValue() && b.WeeklyOff == "None")));
    }

    [Theory]
    [InlineData("Name", "")]
    public async Task Create_requires_name(string _, string name)
    {
        var dto = Fixtures.BranchCreate();
        dto.Name = name;
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateBranchAsync(dto));
        Assert.Equal(ApiMessages.BranchNameRequired, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_unknown_status()
    {
        var dto = Fixtures.BranchCreate();
        dto.Status = "paused";
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateBranchAsync(dto));
        Assert.Equal(ApiMessages.BranchStatusInvalid, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_unknown_service_ids()
    {
        var dto = Fixtures.BranchCreate();
        dto.Services = new List<Guid> { TestIds.Service };
        _branches.Setup(r => r.GetServicesByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(Array.Empty<TheBeautyHubData.Entities.Services>());

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateBranchAsync(dto));
        Assert.Equal(ApiMessages.InvalidServiceIds, ex.Message);
    }

    [Fact]
    public async Task Update_status_only_does_not_require_name()
    {
        _branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        _branches.Setup(r => r.UpdateAsync(It.IsAny<Branch>())).ReturnsAsync((Branch b) => b);

        var saved = await _sut.UpdateStatusAsync(TestIds.Branch, TestIds.Account, "Inactive");

        Assert.True(saved.Saved);
        _branches.Verify(r => r.UpdateAsync(It.Is<Branch>(b => b.Status == RecordStatus.Inactive.ToApiValue())));
    }

    [Fact]
    public async Task Update_status_wrong_account_is_not_found()
    {
        _branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateStatusAsync(TestIds.Branch, Guid.NewGuid(), "active"));
    }

    [Fact]
    public async Task List_returns_mapped_items()
    {
        _branches.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.BranchEntity() });
        var list = (await _sut.GetBranchesAsync(TestIds.Account)).ToList();
        Assert.Single(list);
        Assert.Equal("JARGON Chehadi", list[0].Name);
    }

    [Fact]
    public async Task Details_returns_null_when_missing()
    {
        _branches.Setup(r => r.GetDetailsByIdAsync(TestIds.Branch)).ReturnsAsync((Branch?)null);
        Assert.Null(await _sut.GetBranchDetailsAsync(TestIds.Branch, TestIds.Account));
    }
}
