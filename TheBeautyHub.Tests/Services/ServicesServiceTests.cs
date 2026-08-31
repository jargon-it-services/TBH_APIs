using TheBeautyHubCore.Constants;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests.Services;

public class ServicesServiceTests
{
    private readonly Mock<IServicesRepository> _repo = new();
    private readonly ServicesService _sut;

    public ServicesServiceTests()
    {
        _sut = new ServicesService(_repo.Object);
    }

    private void StubCreateLookups()
    {
        _repo.Setup(r => r.GetBranchesByIdsAsync(TestIds.Account, It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync((Guid _, IEnumerable<Guid> ids) =>
                ids.Select(id => new Branch { BranchId = id, AccountId = TestIds.Account, Name = "B" }).ToList());
        _repo.Setup(r => r.InsertAsync(It.IsAny<TheBeautyHubData.Entities.Services>()))
            .ReturnsAsync((TheBeautyHubData.Entities.Services s) => { s.ServiceId = TestIds.Service; return s; });
        _repo.Setup(r => r.ReplaceBranchesAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
            .Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_accepts_android_service_type_and_percentage()
    {
        StubCreateLookups();
        await _sut.CreateAsync(Fixtures.ServiceCreate());
        _repo.Verify(r => r.InsertAsync(It.Is<TheBeautyHubData.Entities.Services>(s =>
            s.OfferingType == ServiceOfferingType.InSalon.ToApiValue()
            && s.ApplicableGender == ServiceGender.Unisex.ToApiValue()
            && s.CommissionType == CommissionType.Percentage.ToApiValue()
            && s.Status == RecordStatus.Active.ToApiValue())));
    }

    [Fact]
    public async Task Create_allows_empty_description()
    {
        StubCreateLookups();
        var dto = Fixtures.ServiceCreate();
        dto.Description = null;
        await _sut.CreateAsync(dto);
        _repo.Verify(r => r.InsertAsync(It.Is<TheBeautyHubData.Entities.Services>(s => s.ServiceDescription == null)));
    }

    [Fact]
    public async Task Create_all_branches_skips_branch_ids()
    {
        StubCreateLookups();
        await _sut.CreateAsync(Fixtures.ServiceCreate(allBranches: true));
        _repo.Verify(r => r.GetBranchesByIdsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()), Times.Never);
        _repo.Verify(r => r.ReplaceBranchesAsync(TestIds.Service, It.Is<IEnumerable<Guid>>(ids => !ids.Any())));
    }

    [Fact]
    public async Task Create_without_branches_when_not_all_fails()
    {
        var dto = Fixtures.ServiceCreate();
        dto.AllBranches = false;
        dto.Branches = new List<Guid>();
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.BranchesRequiredWhenNotAll, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_unknown_type()
    {
        var dto = Fixtures.ServiceCreate();
        dto.Type = "catalog";
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.ServiceTypeInvalid, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_percentage_over_100()
    {
        var dto = Fixtures.ServiceCreate();
        dto.CommissionValue = 101;
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.ServiceCommissionPercentageInvalid, ex.Message);
    }

    [Fact]
    public async Task Update_status_only_keeps_catalog_fields()
    {
        var existing = Fixtures.ServiceEntity();
        existing.ServiceName = "keep-me";
        _repo.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync(existing);
        _repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

        await _sut.UpdateStatusAsync(TestIds.Service, TestIds.Account, "Inactive");

        Assert.Equal("keep-me", existing.ServiceName);
        Assert.Equal(RecordStatus.Inactive.ToApiValue(), existing.Status);
    }

    [Fact]
    public async Task Catalog_marks_active_from_status()
    {
        _repo.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.ServiceEntity() });
        var catalog = await _sut.GetCatalogAsync(TestIds.Account);
        Assert.True(catalog[0].Active);
    }

    [Fact]
    public async Task Delete_missing_throws()
    {
        _repo.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync((TheBeautyHubData.Entities.Services?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteAsync(TestIds.Service, TestIds.Account));
    }

    [Fact]
    public async Task Details_null_when_missing()
    {
        _repo.Setup(r => r.GetDetailsByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync((TheBeautyHubData.Entities.Services?)null);
        Assert.Null(await _sut.GetDetailsAsync(TestIds.Service, TestIds.Account));
    }
}
