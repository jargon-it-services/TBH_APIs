using TheBeautyHubCore.DTOs;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;

namespace TheBeautyHub.Tests.TestData;

public static class Fixtures
{
    public static SaveBranchDto BranchCreate() => new()
    {
        AccountId = TestIds.Account,
        CreatedBy = TestIds.User,
        Name = "JARGON Chehadi",
        AddressLine1 = "chehadi",
        AddressLine2 = "",
        City = "Nashik",
        State = "Maharashtra",
        Pincode = "422210",
        Mobile = "8793052520",
        Email = "branch@example.com",
        BranchType = "salon",
        OpeningTime = "08:02",
        ClosingTime = "22:02",
        WeeklyOff = "None",
        Status = "Active",
        Services = new List<Guid>()
    };

    public static SaveServiceDto ServiceCreate(bool allBranches = false) => new()
    {
        AccountId = TestIds.Account,
        CreatedBy = TestIds.User,
        Name = "haircut",
        Description = "hair cut des",
        Category = "Hair",
        DurationMinutes = 20,
        ApplicableGender = "Unisex",
        Type = "Service",
        Status = "Active",
        CustomerPrice = 120,
        MaterialCost = 20,
        CommissionType = "Percentage",
        CommissionValue = 50,
        OtherCost = 6,
        HomeServiceAvailable = false,
        AllBranches = allBranches,
        Branches = allBranches ? null : new List<Guid> { TestIds.Branch }
    };

    public static SaveExpenseDto ExpenseUpdate() => new()
    {
        AccountId = TestIds.Account,
        Name = "Water Bottle",
        Description = "Water Bottle",
        AllBranches = false,
        Branches = new List<Guid> { TestIds.OtherBranch },
        Status = "active"
    };

    public static SaveSalaryRuleDto SalaryRule(string salaryType = "Fixed Salary") => new()
    {
        AccountId = TestIds.Account,
        Name = "Incentive",
        Description = "Incentive",
        SalaryType = salaryType,
        FixedSalary = 10000,
        AllowAdvanceRecovery = false,
        Status = "active"
    };

    public static SaveStaffDto StaffCreate(bool allowAppLogin = true) => new()
    {
        AccountId = TestIds.Account,
        CreatedBy = TestIds.User,
        FullName = "Test Staff",
        Mobile = "9999999999",
        Email = "staff@example.com",
        Gender = "Female",
        AadhaarNumber = "123412341234",
        Designation = "Stylist",
        Specialist = "Hair",
        BranchId = TestIds.Branch,
        SalaryRuleId = TestIds.SalaryRule,
        Status = "Active",
        AllowAppLogin = allowAppLogin,
        AppRole = allowAppLogin ? "staff" : null,
        Username = null
    };

    public static SaveTransactionDto Sale() => new()
    {
        AccountId = TestIds.Account,
        UserId = TestIds.User,
        IdempotencyKey = "idem-1",
        Type = "sale",
        BranchId = TestIds.Branch,
        PaymentMode = "upi",
        Services = new List<SaveTransactionLineDto>
        {
            new() { ServiceId = TestIds.Service, Quantity = 1, StaffId = TestIds.Staff }
        }
    };

    public static Branch BranchEntity() => new()
    {
        BranchId = TestIds.Branch,
        AccountId = TestIds.Account,
        Name = "JARGON Chehadi",
        AddressLine1 = "chehadi",
        City = "Nashik",
        State = "Maharashtra",
        Pincode = "422210",
        Mobile = "8793052520",
        Email = "branch@example.com",
        BranchType = "salon",
        OpeningTime = "08:02",
        ClosingTime = "22:02",
        WeeklyOff = "None",
        Status = RecordStatus.Active.ToApiValue()
    };

    public static TheBeautyHubData.Entities.Services ServiceEntity() => new()
    {
        ServiceId = TestIds.Service,
        AccountId = TestIds.Account,
        ServiceName = "haircut",
        Category = "Hair",
        DurationMinutes = 20,
        ApplicableGender = ServiceGender.Unisex.ToApiValue(),
        OfferingType = ServiceOfferingType.InSalon.ToApiValue(),
        Status = RecordStatus.Active.ToApiValue(),
        ServicePrice = 120,
        AllBranches = true
    };

    public static ExpensesType ExpenseEntity() => new()
    {
        ExpensesTypeId = TestIds.Expense,
        AccountId = TestIds.Account,
        ExpensesTypeName = "Water Bottle",
        Description = "Water Bottle",
        AllBranches = true,
        Status = RecordStatus.Active.ToApiValue()
    };

    public static SalaryRule SalaryRuleEntity(string storedType = "fixed_plus_target") => new()
    {
        SalaryRuleId = TestIds.SalaryRule,
        AccountId = TestIds.Account,
        Name = "Fixed + Target Bonus",
        Description = "Fixed + Target Bonus",
        SalaryType = storedType,
        Status = RecordStatus.Active.ToApiValue(),
        IsActive = true
    };

    public static Staff StaffEntity() => new()
    {
        StaffId = TestIds.Staff,
        AccountId = TestIds.Account,
        FullName = "Test Staff",
        Mobile = "9999999999",
        Email = "staff@example.com",
        Gender = PersonGender.Female.ToApiValue(),
        AadhaarNumber = "123412341234",
        Designation = "Stylist",
        Specialist = "Hair",
        BranchId = TestIds.Branch,
        SalaryRuleId = TestIds.SalaryRule,
        Status = RecordStatus.Active.ToApiValue()
    };
}
