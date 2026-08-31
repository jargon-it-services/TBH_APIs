using TheBeautyHubCore.Constants;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests.Services;

public class SalaryRuleServiceTests
{
    private readonly Mock<IStaffRepository> _repo = new();
    private readonly SalaryRuleService _sut;

    public SalaryRuleServiceTests() => _sut = new SalaryRuleService(_repo.Object);

    [Theory]
    [InlineData("Fixed Salary", "Fixed Salary")]
    [InlineData("fixed", "Fixed Salary")]
    [InlineData("Hybrid", "Hybrid")]
    [InlineData("fixed_plus_target", "Hybrid")]
    [InlineData("Service Commission", "Service Commission")]
    [InlineData("Incentive", "Service Commission")]
    public async Task Create_normalizes_android_and_legacy_salary_types(string input, string stored)
    {
        SalaryRule? saved = null;
        _repo.Setup(r => r.InsertSalaryRuleAsync(It.IsAny<SalaryRule>()))
            .ReturnsAsync((SalaryRule r) => { saved = r; r.SalaryRuleId = TestIds.SalaryRule; return r; });

        await _sut.CreateAsync(Fixtures.SalaryRule(input));

        Assert.Equal(stored, saved!.SalaryType);
    }

    [Fact]
    public async Task Details_maps_legacy_stored_type_to_android_label()
    {
        _repo.Setup(r => r.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account))
            .ReturnsAsync(Fixtures.SalaryRuleEntity("fixed_plus_target"));

        var detail = await _sut.GetDetailsAsync(TestIds.SalaryRule, TestIds.Account);

        Assert.Equal("Hybrid", detail!.SalaryType);
    }

    [Fact]
    public async Task List_maps_legacy_types()
    {
        _repo.Setup(r => r.GetSalaryRulesAsync(TestIds.Account))
            .ReturnsAsync(new[] { Fixtures.SalaryRuleEntity("commission") });
        var list = await _sut.GetListAsync(TestIds.Account);
        Assert.Equal("Service Commission", list[0].SalaryType);
    }

    [Fact]
    public async Task Catalog_treats_active_status_as_active()
    {
        _repo.Setup(r => r.GetSalaryRulesAsync(TestIds.Account))
            .ReturnsAsync(new[] { Fixtures.SalaryRuleEntity() });
        var catalog = await _sut.GetCatalogAsync(TestIds.Account);
        Assert.True(catalog[0].Active);
    }

    [Fact]
    public async Task Unknown_salary_type_is_rejected()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(Fixtures.SalaryRule("Hourly")));
        Assert.Equal(ApiMessages.SalaryRuleTypeInvalid, ex.Message);
    }

    [Fact]
    public async Task Update_status_sets_is_active()
    {
        var existing = Fixtures.SalaryRuleEntity();
        _repo.Setup(r => r.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account)).ReturnsAsync(existing);
        _repo.Setup(r => r.UpdateSalaryRuleAsync(existing)).Returns(Task.CompletedTask);

        await _sut.UpdateStatusAsync(TestIds.SalaryRule, TestIds.Account, "Inactive");

        Assert.Equal(RecordStatus.Inactive.ToApiValue(), existing.Status);
        Assert.False(existing.IsActive);
    }

    [Fact]
    public async Task Delete_missing_throws()
    {
        _repo.Setup(r => r.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account)).ReturnsAsync((SalaryRule?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteAsync(TestIds.SalaryRule, TestIds.Account));
    }
}
