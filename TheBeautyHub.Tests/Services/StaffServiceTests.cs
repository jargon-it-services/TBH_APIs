using TheBeautyHubCore.Constants;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests.Services;

public class StaffServiceTests
{
    private readonly Mock<IStaffRepository> _staff = new();
    private readonly Mock<IBranchRepository> _branches = new();
    private readonly StaffService _sut;

    public StaffServiceTests() => _sut = new StaffService(_staff.Object, _branches.Object);

    private void StubLookups()
    {
        _branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        _staff.Setup(r => r.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account)).ReturnsAsync(Fixtures.SalaryRuleEntity());
        _staff.Setup(r => r.EmployeeCodeExistsAsync(TestIds.Account, It.IsAny<string>(), It.IsAny<Guid?>()))
            .ReturnsAsync(false);
        _staff.Setup(r => r.InsertAsync(It.IsAny<Staff>()))
            .ReturnsAsync((Staff s) => { s.StaffId = TestIds.Staff; return s; });
        _staff.Setup(r => r.UpdateAsync(It.IsAny<Staff>())).ReturnsAsync((Staff s) => s);
    }

    [Fact]
    public async Task Create_with_app_login_uses_email_when_username_omitted()
    {
        StubLookups();
        Staff? saved = null;
        _staff.Setup(r => r.InsertAsync(It.IsAny<Staff>()))
            .ReturnsAsync((Staff s) => { saved = s; s.StaffId = TestIds.Staff; return s; });

        await _sut.CreateAsync(Fixtures.StaffCreate());

        Assert.Equal("staff@example.com", saved!.Username);
        Assert.Equal(PersonGender.Female.ToApiValue(), saved.Gender);
        Assert.Equal(RecordStatus.Active.ToApiValue(), saved.Status);
    }

    [Fact]
    public async Task Create_app_login_without_username_or_email_fails()
    {
        StubLookups();
        var dto = Fixtures.StaffCreate();
        dto.Email = " ";
        dto.Username = null;
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.StaffEmailRequired, ex.Message);
    }

    [Fact]
    public async Task Create_app_login_requires_role()
    {
        var dto = Fixtures.StaffCreate();
        dto.AppRole = null;
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.StaffAppRoleRequired, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_invalid_gender()
    {
        var dto = Fixtures.StaffCreate(allowAppLogin: false);
        dto.Gender = "Unknown";
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.StaffGenderInvalid, ex.Message);
    }

    [Fact]
    public async Task Next_employee_code_increments()
    {
        _staff.Setup(r => r.GetEmployeeCodesAsync(TestIds.Account)).ReturnsAsync(new[] { "EMP001", "EMP007" });
        Assert.Equal("EMP008", await _sut.GetNextEmployeeCodeAsync(TestIds.Account));
    }

    [Fact]
    public async Task Next_employee_code_starts_at_001()
    {
        _staff.Setup(r => r.GetEmployeeCodesAsync(TestIds.Account)).ReturnsAsync(Array.Empty<string>());
        Assert.Equal("EMP001", await _sut.GetNextEmployeeCodeAsync(TestIds.Account));
    }

    [Fact]
    public async Task Form_config_includes_default_specialists()
    {
        _staff.Setup(r => r.EnsureDefaultSalaryRulesAsync(TestIds.Account)).Returns(Task.CompletedTask);
        _branches.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.BranchEntity() });
        _staff.Setup(r => r.GetSalaryRulesAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.SalaryRuleEntity() });
        _staff.Setup(r => r.GetSpecialistsAsync(TestIds.Account)).ReturnsAsync(new[] { "Bridal" });

        var config = await _sut.GetFormConfigAsync(TestIds.Account);

        Assert.Contains("Hair", config.Specialists);
        Assert.Contains("Bridal", config.Specialists);
        Assert.Single(config.Branches);
    }

    [Fact]
    public async Task Update_status_does_not_clear_name()
    {
        var existing = Fixtures.StaffEntity();
        _staff.Setup(r => r.GetByIdAsync(TestIds.Staff, TestIds.Account)).ReturnsAsync(existing);
        _staff.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

        await _sut.UpdateStatusAsync(TestIds.Staff, TestIds.Account, "inactive");

        Assert.Equal("Test Staff", existing.FullName);
        Assert.Equal(RecordStatus.Inactive.ToApiValue(), existing.Status);
    }

    [Fact]
    public async Task Duplicate_employee_code_is_rejected()
    {
        StubLookups();
        var dto = Fixtures.StaffCreate(allowAppLogin: false);
        dto.EmployeeCode = "EMP001";
        _staff.Setup(r => r.EmployeeCodeExistsAsync(TestIds.Account, "EMP001", null)).ReturnsAsync(true);
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.StaffEmployeeCodeExists, ex.Message);
    }
}
