using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;
using BranchAppService = TheBeautyHubCore.Services.BranchService;

namespace TheBeautyHub.Tests.Services;

public class RequiredFieldScenarioTests
{
    [Theory]
    [InlineData(nameof(SaveBranchDto.Name), ApiMessages.BranchNameRequired)]
    [InlineData(nameof(SaveBranchDto.AddressLine1), ApiMessages.BranchAddressRequired)]
    [InlineData(nameof(SaveBranchDto.City), ApiMessages.BranchCityRequired)]
    [InlineData(nameof(SaveBranchDto.State), ApiMessages.BranchStateRequired)]
    [InlineData(nameof(SaveBranchDto.Pincode), ApiMessages.BranchPincodeRequired)]
    [InlineData(nameof(SaveBranchDto.Mobile), ApiMessages.BranchMobileRequired)]
    [InlineData(nameof(SaveBranchDto.Email), ApiMessages.BranchEmailRequired)]
    [InlineData(nameof(SaveBranchDto.BranchType), ApiMessages.BranchTypeRequired)]
    [InlineData(nameof(SaveBranchDto.OpeningTime), ApiMessages.BranchOpeningTimeRequired)]
    [InlineData(nameof(SaveBranchDto.ClosingTime), ApiMessages.BranchClosingTimeRequired)]
    [InlineData(nameof(SaveBranchDto.WeeklyOff), ApiMessages.BranchWeeklyOffRequired)]
    [InlineData(nameof(SaveBranchDto.Status), ApiMessages.BranchStatusRequired)]
    public async Task Branch_create_requires_each_field(string property, string message)
    {
        var dto = Fixtures.BranchCreate();
        typeof(SaveBranchDto).GetProperty(property)!.SetValue(dto, " ");
        var sut = new BranchAppService(new Mock<IBranchRepository>().Object, new Mock<IStaffRepository>().Object);
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateBranchAsync(dto));
        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public async Task Branch_create_requires_account_and_rejects_long_times()
    {
        var repo = new Mock<IBranchRepository>();
        var sut = new BranchAppService(repo.Object, new Mock<IStaffRepository>().Object);
        var noAccount = Fixtures.BranchCreate();
        noAccount.AccountId = Guid.Empty;
        Assert.Equal(ApiMessages.AccountRequired,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateBranchAsync(noAccount))).Message);

        var longTime = Fixtures.BranchCreate();
        longTime.OpeningTime = "12:00:00.00";
        Assert.Equal(ApiMessages.BranchTimeTooLong,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateBranchAsync(longTime))).Message);
    }

    [Fact]
    public async Task Branch_update_replaces_services_and_clears_optional_logo()
    {
        var repo = new Mock<IBranchRepository>();
        var existing = Fixtures.BranchEntity();
        existing.Logo = "/uploads/branches/old.png";
        repo.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(existing);
        repo.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);
        repo.Setup(r => r.GetServicesByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new[] { Fixtures.ServiceEntity() });
        repo.Setup(r => r.ReplaceServicesAsync(TestIds.Branch, It.IsAny<IEnumerable<Guid>>())).Returns(Task.CompletedTask);
        var sut = new BranchAppService(repo.Object, new Mock<IStaffRepository>().Object);
        var dto = Fixtures.BranchCreate();
        dto.Services = new List<Guid> { TestIds.Service };
        dto.RemoveLogo = true;
        dto.AddressLine2 = "  ";
        dto.MapsLink = "  ";
        await sut.UpdateBranchAsync(TestIds.Branch, TestIds.Account, dto);
        Assert.Null(existing.Logo);
        Assert.Null(existing.AddressLine2);
        repo.Verify(r => r.ReplaceServicesAsync(TestIds.Branch, It.Is<IEnumerable<Guid>>(ids => ids.Single() == TestIds.Service)));
    }

    [Fact]
    public async Task Branch_details_wrong_account_is_null_and_list_joins_address()
    {
        var repo = new Mock<IBranchRepository>();
        var branch = Fixtures.BranchEntity();
        branch.AddressLine2 = "floor 2";
        repo.Setup(r => r.GetDetailsByIdAsync(TestIds.Branch)).ReturnsAsync(branch);
        repo.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(new[] { branch });
        var staff = new Mock<IStaffRepository>();
        staff.Setup(s => s.GetAllAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Staff>());
        var sut = new BranchAppService(repo.Object, staff.Object);
        Assert.Null(await sut.GetBranchDetailsAsync(TestIds.Branch, Guid.NewGuid()));
        var list = (await sut.GetBranchesAsync(TestIds.Account)).Single();
        Assert.Contains("floor 2", list.Address);
    }

    [Theory]
    [InlineData(nameof(SaveServiceDto.Name), ApiMessages.ServiceNameRequired)]
    [InlineData(nameof(SaveServiceDto.Category), ApiMessages.ServiceCategoryRequired)]
    [InlineData(nameof(SaveServiceDto.ApplicableGender), ApiMessages.ServiceGenderRequired)]
    [InlineData(nameof(SaveServiceDto.Type), ApiMessages.ServiceTypeRequired)]
    [InlineData(nameof(SaveServiceDto.Status), ApiMessages.ServiceStatusRequired)]
    [InlineData(nameof(SaveServiceDto.CommissionType), ApiMessages.ServiceCommissionTypeRequired)]
    public async Task Service_create_requires_each_field(string property, string message)
    {
        var dto = Fixtures.ServiceCreate(allBranches: true);
        typeof(SaveServiceDto).GetProperty(property)!.SetValue(dto, " ");
        var sut = new ServicesService(new Mock<IServicesRepository>().Object);
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto));
        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public async Task Service_create_rejects_negative_amounts_and_unknown_gender()
    {
        var sut = new ServicesService(new Mock<IServicesRepository>().Object);
        async Task AssertMsg(Action<SaveServiceDto> mutate, string message)
        {
            var dto = Fixtures.ServiceCreate(allBranches: true);
            mutate(dto);
            Assert.Equal(message, (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto))).Message);
        }

        await AssertMsg(d => d.DurationMinutes = -1, ApiMessages.ServiceDurationInvalid);
        await AssertMsg(d => d.CustomerPrice = -1, ApiMessages.ServiceCustomerPriceInvalid);
        await AssertMsg(d => d.MaterialCost = -1, ApiMessages.ServiceMaterialCostInvalid);
        await AssertMsg(d => d.OtherCost = -1, ApiMessages.ServiceOtherCostInvalid);
        await AssertMsg(d => { d.CommissionType = "Fixed Amount"; d.CommissionValue = -1; }, ApiMessages.ServiceCommissionValueInvalid);
        await AssertMsg(d => d.ApplicableGender = "Alien", ApiMessages.ServiceGenderInvalid);
        await AssertMsg(d => d.CommissionType = "points", ApiMessages.ServiceCommissionTypeInvalid);
    }

    [Fact]
    public async Task Service_update_not_found_and_invalid_branch_ids()
    {
        var repo = new Mock<IServicesRepository>();
        var sut = new ServicesService(repo.Object);
        repo.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync((TheBeautyHubData.Entities.Services?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateAsync(TestIds.Service, Fixtures.ServiceCreate()));

        var dto = Fixtures.ServiceCreate();
        repo.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync(Fixtures.ServiceEntity());
        repo.Setup(r => r.GetBranchesByIdsAsync(TestIds.Account, It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(Array.Empty<Branch>());
        Assert.Equal(ApiMessages.InvalidBranchIds,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.UpdateAsync(TestIds.Service, dto))).Message);
    }

    [Fact]
    public async Task Service_delete_and_fixed_commission_apply()
    {
        var repo = new Mock<IServicesRepository>();
        var existing = Fixtures.ServiceEntity();
        repo.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync(existing);
        repo.Setup(r => r.RemoveBranchLinksAsync(TestIds.Service)).Returns(Task.CompletedTask);
        repo.Setup(r => r.SoftDeleteAsync(existing)).Returns(Task.CompletedTask);
        repo.Setup(r => r.GetBranchesByIdsAsync(TestIds.Account, It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new[] { Fixtures.BranchEntity() });
        repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
        repo.Setup(r => r.ReplaceBranchesAsync(TestIds.Service, It.IsAny<IEnumerable<Guid>>())).Returns(Task.CompletedTask);
        var sut = new ServicesService(repo.Object);
        await sut.DeleteAsync(TestIds.Service, TestIds.Account);
        repo.Verify(r => r.SoftDeleteAsync(existing));

        var dto = Fixtures.ServiceCreate();
        dto.CommissionType = "Fixed Amount";
        dto.CommissionValue = 25;
        await sut.UpdateAsync(TestIds.Service, dto);
        Assert.Equal(25, existing.IncentiveAmount);
        Assert.Null(existing.IncentivePercentage);
    }

    [Theory]
    [InlineData(nameof(SaveStaffDto.FullName), ApiMessages.StaffFullNameRequired)]
    [InlineData(nameof(SaveStaffDto.Mobile), ApiMessages.StaffMobileRequired)]
    [InlineData(nameof(SaveStaffDto.Email), ApiMessages.StaffEmailRequired)]
    [InlineData(nameof(SaveStaffDto.Gender), ApiMessages.StaffGenderRequired)]
    [InlineData(nameof(SaveStaffDto.AadhaarNumber), ApiMessages.StaffAadhaarRequired)]
    [InlineData(nameof(SaveStaffDto.Designation), ApiMessages.StaffDesignationRequired)]
    [InlineData(nameof(SaveStaffDto.Specialist), ApiMessages.StaffSpecialistRequired)]
    [InlineData(nameof(SaveStaffDto.Status), ApiMessages.StaffStatusRequired)]
    public async Task Staff_create_requires_each_field(string property, string message)
    {
        var dto = Fixtures.StaffCreate(allowAppLogin: false);
        typeof(SaveStaffDto).GetProperty(property)!.SetValue(dto, " ");
        var sut = new StaffService(new Mock<IStaffRepository>().Object, new Mock<IBranchRepository>().Object);
        Assert.Equal(message, (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto))).Message);
    }

    [Fact]
    public async Task Staff_rejects_empty_branch_rule_account_and_bad_joining_date()
    {
        var sut = new StaffService(new Mock<IStaffRepository>().Object, new Mock<IBranchRepository>().Object);
        async Task AssertMsg(Action<SaveStaffDto> mutate, string message)
        {
            var dto = Fixtures.StaffCreate(allowAppLogin: false);
            mutate(dto);
            Assert.Equal(message, (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto))).Message);
        }

        await AssertMsg(d => d.AccountId = Guid.Empty, ApiMessages.AccountRequired);
        await AssertMsg(d => d.BranchId = Guid.Empty, ApiMessages.StaffBranchRequired);
        await AssertMsg(d => d.SalaryRuleId = Guid.Empty, ApiMessages.StaffSalaryRuleRequired);

        var staff = new Mock<IStaffRepository>();
        var branches = new Mock<IBranchRepository>();
        branches.Setup(b => b.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        staff.Setup(s => s.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account)).ReturnsAsync(Fixtures.SalaryRuleEntity());
        staff.Setup(s => s.EmployeeCodeExistsAsync(TestIds.Account, It.IsAny<string>(), It.IsAny<Guid?>())).ReturnsAsync(false);
        var withLookups = new StaffService(staff.Object, branches.Object);
        var badDate = Fixtures.StaffCreate(allowAppLogin: false);
        badDate.JoiningDate = "not-a-date";
        Assert.Equal(ApiMessages.StaffJoiningDateInvalid,
            (await Assert.ThrowsAsync<ArgumentException>(() => withLookups.CreateAsync(badDate))).Message);
    }

    [Fact]
    public async Task Staff_create_rejects_unknown_branch_and_salary_rule()
    {
        var staff = new Mock<IStaffRepository>();
        var branches = new Mock<IBranchRepository>();
        var sut = new StaffService(staff.Object, branches.Object);
        var dto = Fixtures.StaffCreate(allowAppLogin: false);
        branches.Setup(b => b.GetByIdAsync(TestIds.Branch)).ReturnsAsync((Branch?)null);
        Assert.Equal(ApiMessages.StaffBranchInvalid,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto))).Message);

        branches.Setup(b => b.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        staff.Setup(s => s.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account)).ReturnsAsync((SalaryRule?)null);
        Assert.Equal(ApiMessages.StaffSalaryRuleInvalid,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto))).Message);
    }

    [Fact]
    public async Task Staff_update_delete_and_details()
    {
        var staffRepo = new Mock<IStaffRepository>();
        var branches = new Mock<IBranchRepository>();
        var existing = Fixtures.StaffEntity();
        existing.UserId = TestIds.User;
        staffRepo.Setup(s => s.GetByIdAsync(TestIds.Staff, TestIds.Account)).ReturnsAsync(existing);
        staffRepo.Setup(s => s.GetByIdAsync(TestIds.OtherBranch, TestIds.Account)).ReturnsAsync((Staff?)null);
        staffRepo.Setup(s => s.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account)).ReturnsAsync(Fixtures.SalaryRuleEntity());
        staffRepo.Setup(s => s.EmployeeCodeExistsAsync(TestIds.Account, It.IsAny<string>(), It.IsAny<Guid?>())).ReturnsAsync(false);
        staffRepo.Setup(s => s.UpdateAsync(existing)).ReturnsAsync(existing);
        staffRepo.Setup(s => s.AssignBranchEmployeeAsync(TestIds.User, TestIds.Branch, It.IsAny<string?>())).Returns(Task.CompletedTask);
        staffRepo.Setup(s => s.RemoveBranchEmployeesForUserAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        staffRepo.Setup(s => s.SoftDeleteAsync(existing)).Returns(Task.CompletedTask);
        branches.Setup(b => b.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        var sut = new StaffService(staffRepo.Object, branches.Object);

        var dto = Fixtures.StaffCreate(allowAppLogin: false);
        dto.JoiningDate = "2026-01-15";
        dto.RemovePhoto = true;
        await sut.UpdateAsync(TestIds.Staff, dto);
        Assert.Null(existing.Photo);
        Assert.NotNull(existing.JoiningDate);

        await sut.DeleteAsync(TestIds.Staff, TestIds.Account);
        staffRepo.Verify(s => s.SoftDeleteAsync(existing));
        Assert.Null(await sut.GetDetailsAsync(TestIds.OtherBranch, TestIds.Account));
        Assert.NotNull(await sut.GetDetailsAsync(TestIds.Staff, TestIds.Account));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateStatusAsync(TestIds.OtherBranch, TestIds.Account, "Active"));
    }

    [Theory]
    [InlineData(nameof(SaveSalaryRuleDto.Name), ApiMessages.SalaryRuleNameRequired)]
    [InlineData(nameof(SaveSalaryRuleDto.SalaryType), ApiMessages.SalaryRuleTypeRequired)]
    [InlineData(nameof(SaveSalaryRuleDto.Status), ApiMessages.SalaryRuleStatusRequired)]
    public async Task Salary_rule_requires_fields(string property, string message)
    {
        var dto = Fixtures.SalaryRule();
        typeof(SaveSalaryRuleDto).GetProperty(property)!.SetValue(dto, " ");
        var sut = new SalaryRuleService(new Mock<IStaffRepository>().Object);
        Assert.Equal(message, (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto))).Message);
    }

    [Fact]
    public async Task Salary_rule_update_missing_and_details_null()
    {
        var repo = new Mock<IStaffRepository>();
        repo.Setup(r => r.GetSalaryRuleAsync(TestIds.SalaryRule, TestIds.Account)).ReturnsAsync((SalaryRule?)null);
        var sut = new SalaryRuleService(repo.Object);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateAsync(TestIds.SalaryRule, Fixtures.SalaryRule()));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateStatusAsync(TestIds.SalaryRule, TestIds.Account, "Active"));
        Assert.Null(await sut.GetDetailsAsync(TestIds.SalaryRule, TestIds.Account));
    }

    [Theory]
    [InlineData(nameof(SaveExpenseDto.Name), ApiMessages.ExpenseNameRequired)]
    [InlineData(nameof(SaveExpenseDto.Status), ApiMessages.ExpenseStatusRequired)]
    public async Task Expense_requires_fields(string property, string message)
    {
        var dto = Fixtures.ExpenseUpdate();
        dto.AllBranches = true;
        typeof(SaveExpenseDto).GetProperty(property)!.SetValue(dto, " ");
        var sut = new ExpensesTypeService(new Mock<IExpensesTypeRepository>().Object);
        Assert.Equal(message, (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto))).Message);
    }

    [Fact]
    public async Task Expense_invalid_branch_ids_and_delete()
    {
        var repo = new Mock<IExpensesTypeRepository>();
        var sut = new ExpensesTypeService(repo.Object);
        var dto = Fixtures.ExpenseUpdate();
        repo.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync(Fixtures.ExpenseEntity());
        repo.Setup(r => r.GetBranchesByIdsAsync(TestIds.Account, It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(Array.Empty<Branch>());
        Assert.Equal(ApiMessages.InvalidBranchIds,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.UpdateAsync(TestIds.Expense, dto))).Message);

        var existing = Fixtures.ExpenseEntity();
        repo.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync(existing);
        repo.Setup(r => r.RemoveBranchLinksAsync(TestIds.Expense)).Returns(Task.CompletedTask);
        repo.Setup(r => r.SoftDeleteAsync(existing)).Returns(Task.CompletedTask);
        await sut.DeleteAsync(TestIds.Expense, TestIds.Account);
        repo.Verify(r => r.SoftDeleteAsync(existing));
    }

    [Fact]
    public async Task Transaction_update_succeeds_inside_window_and_requires_payment_and_branch()
    {
        var tx = new Mock<ITransactionRepository>();
        var services = new Mock<IServicesRepository>();
        var expenses = new Mock<IExpensesTypeRepository>();
        var staff = new Mock<IStaffRepository>();
        var branches = new Mock<IBranchRepository>();
        var sut = new TransactionService(tx.Object, services.Object, expenses.Object, staff.Object, branches.Object);

        var missingPay = Fixtures.Sale();
        missingPay.PaymentMode = " ";
        Assert.Equal(ApiMessages.TransactionPaymentModeRequired,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(missingPay))).Message);

        var missingBranch = Fixtures.Sale();
        missingBranch.BranchId = Guid.Empty;
        Assert.Equal(ApiMessages.TransactionBranchRequired,
            (await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(missingBranch))).Message);

        var existing = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Code = "TXN1",
            AccountId = TestIds.Account,
            EditableUntil = DateTime.UtcNow.AddHours(1),
            Status = TransactionStatus.Paid.ToApiValue()
        };
        tx.Setup(r => r.GetDetailsAsync(existing.TransactionId, TestIds.Account)).ReturnsAsync(existing);
        branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        services.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync(Fixtures.ServiceEntity());
        tx.Setup(r => r.ReplaceDetailsAsync(existing.TransactionId, It.IsAny<IEnumerable<TransactionDetail>>())).Returns(Task.CompletedTask);
        tx.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);
        var dto = Fixtures.Sale();
        dto.IdempotencyKey = null;
        dto.CustomerName = "Walk-in";
        var saved = await sut.UpdateAsync(existing.TransactionId.ToString(), dto);
        Assert.Equal("TXN1", saved.Id);
        Assert.Equal(1, existing.EditCount);
        Assert.Equal("Walk-in", existing.CustomerName);
    }
}
