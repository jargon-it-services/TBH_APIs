using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Controllers;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests.Controllers;

public class LiveApiHttpTests
{
    private static IAuthCenterUserLookup AuthUsers()
    {
        var mock = new Mock<IAuthCenterUserLookup>();
        mock.Setup(a => a.ResolveCurrentUserIdAsync(It.IsAny<CancellationToken>())).ReturnsAsync(TestIds.User);
        mock.Setup(a => a.ResolveUserIdAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestIds.User);
        return mock.Object;
    }

    [Fact]
    public async Task Management_account_summary_and_feature_lock_succeed()
    {
        var management = new Mock<IManagementService>();
        management.Setup(s => s.GetAccountSummaryAsync(TestIds.Account))
            .ReturnsAsync(new AccountSummaryDto { TotalBranches = 2, TotalStaff = 3 });
        management.Setup(s => s.GetFeatureLockAsync(TestIds.Account))
            .ReturnsAsync(new FeatureLockDto { FeatureLock = new List<string> { "report" } });
        var controller = ApiTest.Controller(new ManagementController(management.Object, ApiTest.Logs(), ApiTest.User()));

        ApiTest.AssertOk<AccountSummaryDataResponse>(
            await controller.GetAccountSummary(),
            ApiMessages.AccountSummaryFetched,
            d => Assert.Equal(2, d.TotalBranches));
        ApiTest.AssertOk<FeatureLockDataResponse>(
            await controller.GetFeatureLock(),
            ApiMessages.FeatureLockFetched,
            d => Assert.Equal(new[] { "report" }, d.FeatureLock));
    }

    [Fact]
    public async Task Management_failures_are_500()
    {
        var management = new Mock<IManagementService>();
        management.Setup(s => s.GetAccountSummaryAsync(TestIds.Account)).ThrowsAsync(new InvalidOperationException("db"));
        management.Setup(s => s.GetFeatureLockAsync(TestIds.Account)).ThrowsAsync(new InvalidOperationException("db"));
        var controller = ApiTest.Controller(new ManagementController(management.Object, ApiTest.Logs(), ApiTest.User()));

        ApiTest.AssertFail(await controller.GetAccountSummary(), 500, ApiMessages.AccountSummaryFailed);
        ApiTest.AssertFail(await controller.GetFeatureLock(), 500, ApiMessages.FeatureLockFailed);
    }

    [Fact]
    public async Task Expenses_cover_list_details_create_status_update_delete_and_errors()
    {
        var service = new Mock<IExpensesTypeService>();
        service.Setup(s => s.GetListAsync(TestIds.Account)).ReturnsAsync(new[]
        {
            new ExpenseListItemDto { Id = TestIds.Expense, Name = "Water", Status = "Active" }
        });
        service.Setup(s => s.GetDetailsAsync(TestIds.Expense, TestIds.Account))
            .ReturnsAsync(new ExpenseDetailDto { Id = TestIds.Expense, Name = "Water" });
        service.Setup(s => s.GetDetailsAsync(TestIds.OtherBranch, TestIds.Account)).ReturnsAsync((ExpenseDetailDto?)null);
        service.Setup(s => s.CreateAsync(It.IsAny<SaveExpenseDto>())).Returns(Task.CompletedTask);
        service.Setup(s => s.UpdateStatusAsync(TestIds.Expense, TestIds.Account, "Inactive")).Returns(Task.CompletedTask);
        service.Setup(s => s.DeleteAsync(TestIds.Expense, TestIds.Account)).Returns(Task.CompletedTask);

        var controller = ApiTest.Controller(new ExpensesTypesController(service.Object, ApiTest.Logs(), ApiTest.Mapper(), ApiTest.User()));

        ApiTest.AssertOk<ExpenseListDataResponse>(await controller.GetList(), ApiMessages.ExpenseListFetched, d => Assert.Single(d.Expenses));
        ApiTest.AssertOk<ExpenseDetailResponse>(await controller.GetDetails(TestIds.Expense), ApiMessages.ExpenseDetailsFetched);
        ApiTest.AssertNotFound(await controller.GetDetails(TestIds.OtherBranch), ApiMessages.ExpenseNotFound);
        ApiTest.AssertOk<ExpenseSavedDataResponse>(
            await controller.Create(new SaveExpenseRequest { Name = "Water", Status = "active", AllBranches = true }),
            ApiMessages.ExpenseCreated);
        ApiTest.AssertOk<ExpenseSavedDataResponse>(
            await controller.Update(TestIds.Expense, new SaveExpenseRequest { Status = "Inactive" }),
            ApiMessages.ExpenseUpdated);
        service.Verify(s => s.UpdateStatusAsync(TestIds.Expense, TestIds.Account, "Inactive"), Times.Once);
        ApiTest.AssertOk<ExpenseDeletedDataResponse>(await controller.Delete(TestIds.Expense), ApiMessages.ExpenseDeleted);
        ApiTest.AssertNotFound(await controller.Delete(TestIds.OtherBranch), ApiMessages.ExpenseNotFound);

        service.Setup(s => s.GetListAsync(TestIds.Account)).ThrowsAsync(new Exception("boom"));
        ApiTest.AssertFail(await controller.GetList(), 500, ApiMessages.ExpenseListFailed);
        service.Setup(s => s.CreateAsync(It.IsAny<SaveExpenseDto>()))
            .ThrowsAsync(new ArgumentException(ApiMessages.ExpenseNameRequired));
        ApiTest.AssertBadRequest(
            await controller.Create(new SaveExpenseRequest { Name = "x", Status = "active", AllBranches = true }),
            ApiMessages.ExpenseNameRequired);
    }

    [Fact]
    public async Task Salary_rules_cover_catalog_list_details_create_update_delete()
    {
        var service = new Mock<ISalaryRuleService>();
        service.Setup(s => s.GetCatalogAsync(TestIds.Account)).ReturnsAsync(new[]
        {
            new SalaryRuleCatalogItemDto { Id = TestIds.SalaryRule, Name = "Fixed", Active = true }
        });
        service.Setup(s => s.GetListAsync(TestIds.Account)).ReturnsAsync(new[]
        {
            new SalaryRuleListItemDto { Id = TestIds.SalaryRule, Name = "Fixed", SalaryType = "Fixed Salary", Status = "Active" }
        });
        service.Setup(s => s.GetDetailsAsync(TestIds.SalaryRule, TestIds.Account))
            .ReturnsAsync(new SalaryRuleDetailDto { Id = TestIds.SalaryRule, Name = "Fixed", SalaryType = "Fixed Salary" });
        service.Setup(s => s.GetDetailsAsync(TestIds.OtherBranch, TestIds.Account)).ReturnsAsync((SalaryRuleDetailDto?)null);
        service.Setup(s => s.CreateAsync(It.IsAny<SaveSalaryRuleDto>())).Returns(Task.CompletedTask);
        service.Setup(s => s.UpdateStatusAsync(TestIds.SalaryRule, TestIds.Account, "Inactive")).Returns(Task.CompletedTask);
        service.Setup(s => s.DeleteAsync(TestIds.SalaryRule, TestIds.Account)).Returns(Task.CompletedTask);

        var controller = ApiTest.Controller(new SalaryRulesController(service.Object, ApiTest.Logs(), ApiTest.Mapper(), ApiTest.User()));

        ApiTest.AssertOk<SalaryRuleCatalogDataResponse>(await controller.GetCatalog(), ApiMessages.SalaryRuleCatalogFetched, d => Assert.Single(d.SalaryRules));
        ApiTest.AssertOk<SalaryRuleListDataResponse>(await controller.GetList(), ApiMessages.SalaryRuleListFetched);
        ApiTest.AssertOk<SalaryRuleDetailResponse>(await controller.GetDetails(TestIds.SalaryRule), ApiMessages.SalaryRuleDetailsFetched);
        ApiTest.AssertNotFound(await controller.GetDetails(TestIds.OtherBranch), ApiMessages.SalaryRuleNotFound);
        ApiTest.AssertOk<SalaryRuleSavedDataResponse>(
            await controller.Create(new SaveSalaryRuleRequest { Name = "Fixed", SalaryType = "Fixed Salary", Status = "active" }),
            ApiMessages.SalaryRuleCreated);
        ApiTest.AssertOk<SalaryRuleSavedDataResponse>(
            await controller.Update(TestIds.SalaryRule, new SaveSalaryRuleRequest { Status = "Inactive" }),
            ApiMessages.SalaryRuleUpdated);
        service.Verify(s => s.UpdateStatusAsync(TestIds.SalaryRule, TestIds.Account, "Inactive"), Times.Once);
        ApiTest.AssertOk<SalaryRuleDeletedDataResponse>(await controller.Delete(TestIds.SalaryRule), ApiMessages.SalaryRuleDeleted);
        ApiTest.AssertNotFound(await controller.Delete(TestIds.OtherBranch), ApiMessages.SalaryRuleNotFound);
    }

    [Fact]
    public async Task Transactions_cover_bootstrap_list_details_create_update_mark_paid()
    {
        var service = new Mock<ITransactionService>();
        service.Setup(s => s.GetBootstrapAsync(TestIds.Account, TestIds.User, It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(new TransactionBootstrapDto { LoggedInUserId = TestIds.User });
        service.Setup(s => s.GetListAsync(TestIds.Account)).ReturnsAsync(new TransactionListDto());
        service.Setup(s => s.GetDetailsAsync("TXN1", TestIds.Account))
            .ReturnsAsync(new TransactionRecordDto { Id = "TXN1", Status = "paid" });
        service.Setup(s => s.GetDetailsAsync("missing", TestIds.Account)).ReturnsAsync((TransactionRecordDto?)null);
        service.Setup(s => s.CreateAsync(It.IsAny<SaveTransactionDto>()))
            .ReturnsAsync(new TransactionSavedDto { Id = "TXN1", Status = "paid" });
        service.Setup(s => s.UpdateAsync("TXN1", It.IsAny<SaveTransactionDto>()))
            .ReturnsAsync(new TransactionSavedDto { Id = "TXN1" });
        service.Setup(s => s.UpdateAsync("closed", It.IsAny<SaveTransactionDto>()))
            .ThrowsAsync(new InvalidOperationException(TransactionService.EditWindowClosedCode));
        service.Setup(s => s.MarkPaidAsync("TXN1", TestIds.Account))
            .ReturnsAsync(new TransactionSavedDto { Id = "TXN1", Status = "paid", PaidAt = DateTime.UtcNow });
        service.Setup(s => s.MarkPaidAsync("missing", TestIds.Account))
            .ThrowsAsync(new KeyNotFoundException(ApiMessages.TransactionNotFound));

        var controller = ApiTest.Controller(new TransactionsController(service.Object, ApiTest.Logs(), ApiTest.Mapper(), ApiTest.User()));
        var payload = new SaveTransactionRequest
        {
            IdempotencyKey = "idem-1",
            Type = "sale",
            BranchId = TestIds.Branch,
            PaymentMode = "upi",
            Services = new List<SaveTransactionLineRequest> { new() { ServiceId = TestIds.Service, Quantity = 1 } }
        };

        ApiTest.AssertOk<TransactionBootstrapResponse>(await controller.GetBootstrap(), ApiMessages.TransactionBootstrapFetched);
        ApiTest.AssertOk<TransactionListDataResponse>(await controller.GetList(), ApiMessages.TransactionListFetched);
        ApiTest.AssertOk<TransactionRecordResponse>(await controller.GetDetails("TXN1"), ApiMessages.TransactionDetailsFetched);
        ApiTest.AssertNotFound(await controller.GetDetails("missing"), ApiMessages.TransactionNotFound);
        ApiTest.AssertOk<TransactionSavedResponse>(await controller.Create(payload), ApiMessages.TransactionCreated, d => Assert.Equal("TXN1", d.Id));
        ApiTest.AssertBadRequest(await controller.Create(new SaveTransactionRequest { Type = "sale" }), ApiMessages.TransactionIdempotencyRequired);
        ApiTest.AssertOk<TransactionSavedResponse>(await controller.Update("TXN1", payload), ApiMessages.TransactionUpdated);
        ApiTest.AssertConflict(await controller.Update("closed", payload), ApiMessages.TransactionEditWindowClosed);
        ApiTest.AssertOk<TransactionMarkPaidResponse>(await controller.MarkPaid("TXN1"), ApiMessages.TransactionMarkedPaid);
        ApiTest.AssertNotFound(await controller.MarkPaid("missing"), ApiMessages.TransactionNotFound);
    }

    [Fact]
    public async Task Branches_cover_list_details_create_status_update_and_errors()
    {
        var service = new Mock<IBranchService>();
        service.Setup(s => s.GetBranchesAsync(TestIds.Account)).ReturnsAsync(new[]
        {
            new BranchListItemDto { Id = TestIds.Branch, Name = "JARGON Chehadi" }
        });
        service.Setup(s => s.GetBranchDetailsAsync(TestIds.Branch, TestIds.Account))
            .ReturnsAsync(new BranchDetailDto { Id = TestIds.Branch, Name = "JARGON Chehadi" });
        service.Setup(s => s.GetBranchDetailsAsync(TestIds.OtherBranch, TestIds.Account)).ReturnsAsync((BranchDetailDto?)null);
        service.Setup(s => s.CreateBranchAsync(It.IsAny<SaveBranchDto>())).ReturnsAsync(new BranchSavedDto { Saved = true });
        service.Setup(s => s.UpdateStatusAsync(TestIds.Branch, TestIds.Account, "Inactive")).ReturnsAsync(new BranchSavedDto { Saved = true });

        var controller = ApiTest.Controller(new BranchesController(
            service.Object, ApiTest.Logs(), ApiTest.Mapper(), ApiTest.BranchLogos(), ApiTest.User()));

        ApiTest.AssertOk<BranchListDataResponse>(await controller.GetBranches(), ApiMessages.BranchListFetched, d => Assert.Single(d.Branches));
        ApiTest.AssertOk<BranchDetailResponse>(await controller.GetBranchDetails(TestIds.Branch), ApiMessages.BranchDetailsFetched);
        ApiTest.AssertNotFound(await controller.GetBranchDetails(TestIds.OtherBranch), ApiMessages.BranchNotFound);
        ApiTest.AssertOk<BranchSavedDataResponse>(
            await controller.CreateBranchFromJson(new SaveBranchRequest
            {
                Name = "JARGON Chehadi",
                AddressLine1 = "chehadi",
                City = "Nashik",
                State = "MH",
                Pincode = "422210",
                Mobile = "8793052520",
                Email = "b@x.com",
                BranchType = "salon",
                OpeningTime = "08:02",
                ClosingTime = "22:02",
                WeeklyOff = "None",
                Status = "Active"
            }),
            ApiMessages.BranchCreated);
        ApiTest.AssertOk<BranchSavedDataResponse>(
            await controller.UpdateBranchFromJson(TestIds.Branch, new SaveBranchRequest { Status = "Inactive" }),
            ApiMessages.BranchUpdated);
        service.Verify(s => s.UpdateStatusAsync(TestIds.Branch, TestIds.Account, "Inactive"), Times.Once);
        ApiTest.AssertNotFound(
            await controller.UpdateBranchFromJson(TestIds.OtherBranch, new SaveBranchRequest { Status = "Inactive" }),
            ApiMessages.BranchNotFound);

        service.Setup(s => s.GetBranchesAsync(TestIds.Account)).ThrowsAsync(new Exception("boom"));
        ApiTest.AssertFail(await controller.GetBranches(), 500, ApiMessages.BranchListFailed);
    }

    [Fact]
    public async Task Services_cover_catalog_list_details_create_status_delete()
    {
        var service = new Mock<IServicesService>();
        service.Setup(s => s.GetCatalogAsync(TestIds.Account)).ReturnsAsync(new[]
        {
            new ServiceCatalogItemDto { Id = TestIds.Service, Name = "haircut", Active = true }
        });
        service.Setup(s => s.GetListAsync(TestIds.Account)).ReturnsAsync(new[]
        {
            new ServiceListItemDto { Id = TestIds.Service, Name = "haircut", Status = "Active" }
        });
        service.Setup(s => s.GetDetailsAsync(TestIds.Service, TestIds.Account))
            .ReturnsAsync(new ServiceDetailDto { Id = TestIds.Service, Name = "haircut" });
        service.Setup(s => s.GetDetailsAsync(TestIds.OtherBranch, TestIds.Account)).ReturnsAsync((ServiceDetailDto?)null);
        service.Setup(s => s.CreateAsync(It.IsAny<SaveServiceDto>())).Returns(Task.CompletedTask);
        service.Setup(s => s.UpdateStatusAsync(TestIds.Service, TestIds.Account, "Inactive")).Returns(Task.CompletedTask);
        service.Setup(s => s.DeleteAsync(TestIds.Service, TestIds.Account)).Returns(Task.CompletedTask);

        var controller = ApiTest.Controller(new ServicesController(
            service.Object, ApiTest.Logs(), ApiTest.Mapper(), ApiTest.ServicePhotos(), ApiTest.User()));

        ApiTest.AssertOk<ServiceCatalogDataResponse>(await controller.GetCatalog(), ApiMessages.ServiceCatalogFetched);
        ApiTest.AssertOk<ServiceListDataResponse>(await controller.GetList(), ApiMessages.ServiceListFetched);
        ApiTest.AssertOk<ServiceDetailResponse>(await controller.GetDetails(TestIds.Service), ApiMessages.ServiceDetailsFetched);
        ApiTest.AssertNotFound(await controller.GetDetails(TestIds.OtherBranch), ApiMessages.ServiceNotFound);
        ApiTest.AssertOk<ServiceSavedDataResponse>(
            await controller.CreateFromJson(new SaveServiceRequest
            {
                Name = "haircut",
                Category = "Hair",
                DurationMinutes = 20,
                ApplicableGender = "Unisex",
                Type = "Service",
                Status = "Active",
                CustomerPrice = 120,
                CommissionType = "Percentage",
                AllBranches = true
            }),
            ApiMessages.ServiceCreated);
        ApiTest.AssertOk<ServiceSavedDataResponse>(
            await controller.UpdateFromJson(TestIds.Service, new SaveServiceRequest { Status = "Inactive" }),
            ApiMessages.ServiceUpdated);
        service.Verify(s => s.UpdateStatusAsync(TestIds.Service, TestIds.Account, "Inactive"), Times.Once);
        ApiTest.AssertOk<ServiceDeletedDataResponse>(await controller.Delete(TestIds.Service), ApiMessages.ServiceDeleted);
        ApiTest.AssertNotFound(await controller.Delete(TestIds.OtherBranch), ApiMessages.ServiceNotFound);
    }

    [Fact]
    public async Task Staff_cover_form_config_list_code_details_status_delete_and_next_code_fallback()
    {
        var service = new Mock<IStaffService>();
        service.Setup(s => s.GetFormConfigAsync(TestIds.Account)).ReturnsAsync(new StaffFormConfigDto());
        service.Setup(s => s.GetListAsync(TestIds.Account)).ReturnsAsync(new[]
        {
            new StaffListItemDto { Id = TestIds.Staff, FullName = "Test Staff" }
        });
        service.Setup(s => s.GetNextEmployeeCodeAsync(TestIds.Account)).ReturnsAsync("EMP009");
        service.Setup(s => s.GetDetailsAsync(TestIds.Staff, TestIds.Account))
            .ReturnsAsync(new StaffDetailDto { Id = TestIds.Staff, FullName = "Test Staff" });
        service.Setup(s => s.GetDetailsAsync(TestIds.OtherBranch, TestIds.Account)).ReturnsAsync((StaffDetailDto?)null);
        service.Setup(s => s.UpdateStatusAsync(TestIds.Staff, TestIds.Account, "Inactive")).Returns(Task.CompletedTask);
        service.Setup(s => s.DeleteAsync(TestIds.Staff, TestIds.Account)).Returns(Task.CompletedTask);

        var controller = ApiTest.Controller(new StaffController(
            service.Object, ApiTest.Logs(), ApiTest.Mapper(), ApiTest.StaffFiles(), ApiTest.User(), AuthUsers()));

        ApiTest.AssertOk<StaffFormConfigDataResponse>(await controller.GetFormConfig(), ApiMessages.StaffFormConfigFetched);
        ApiTest.AssertOk<StaffListDataResponse>(await controller.GetList(), ApiMessages.StaffListFetched, d => Assert.Single(d.Staff));
        ApiTest.AssertOk<NextEmployeeCodeDataResponse>(await controller.GetNextEmployeeCode(), ApiMessages.StaffNextCodeFetched, d => Assert.Equal("EMP009", d.EmployeeCode));
        ApiTest.AssertOk<StaffDetailResponse>(await controller.GetDetails(TestIds.Staff), ApiMessages.StaffDetailsFetched);
        ApiTest.AssertNotFound(await controller.GetDetails(TestIds.OtherBranch), ApiMessages.StaffNotFound);
        ApiTest.AssertOk<StaffSavedDataResponse>(
            await controller.UpdateFromJson(TestIds.Staff, new SaveStaffRequest { Status = "Inactive" }),
            ApiMessages.StaffUpdated);
        service.Verify(s => s.UpdateStatusAsync(TestIds.Staff, TestIds.Account, "Inactive"), Times.Once);
        ApiTest.AssertOk<StaffDeletedDataResponse>(await controller.Delete(TestIds.Staff), ApiMessages.StaffDeleted);
        ApiTest.AssertNotFound(await controller.Delete(TestIds.OtherBranch), ApiMessages.StaffNotFound);

        service.Setup(s => s.GetNextEmployeeCodeAsync(TestIds.Account)).ThrowsAsync(new Exception("db"));
        var fallback = Assert.IsType<OkObjectResult>(await controller.GetNextEmployeeCode());
        var body = Assert.IsType<ApiStatusResponse<NextEmployeeCodeDataResponse>>(fallback.Value);
        Assert.True(body.Status);
        Assert.Null(body.Data!.EmployeeCode);
    }
}
