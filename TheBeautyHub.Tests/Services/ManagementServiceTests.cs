using TheBeautyHubCore.Enums;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests.Services;

public class ManagementServiceTests
{
    private readonly Mock<IBranchRepository> _branches = new();
    private readonly Mock<IStaffRepository> _staff = new();
    private readonly Mock<IServicesRepository> _services = new();
    private readonly Mock<IExpensesTypeRepository> _expenses = new();
    private readonly Mock<ISubscriptionRepository> _subs = new();
    private readonly ManagementService _sut;

    public ManagementServiceTests()
        => _sut = new ManagementService(_branches.Object, _staff.Object, _services.Object, _expenses.Object, _subs.Object);

    [Fact]
    public async Task Account_summary_counts_each_catalog()
    {
        _branches.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.BranchEntity() });
        _staff.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.StaffEntity() });
        _services.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.ServiceEntity() });
        _expenses.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.ExpenseEntity() });
        _staff.Setup(r => r.GetSalaryRulesAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.SalaryRuleEntity() });

        var summary = await _sut.GetAccountSummaryAsync(TestIds.Account);

        Assert.Equal(1, summary.TotalBranches);
        Assert.Equal(1, summary.TotalStaff);
        Assert.Equal(1, summary.TotalServices);
        Assert.Equal(1, summary.TotalExpenses);
        Assert.Equal(1, summary.TotalSalaryRules);
    }

    [Fact]
    public async Task Feature_lock_applies_free_trial_defaults_when_unsubscribed()
    {
        _subs.Setup(r => r.GetActiveSubscriptionsByAccountIdAsync(TestIds.Account))
            .ReturnsAsync(Array.Empty<Subscription>());

        var locks = await _sut.GetFeatureLockAsync(TestIds.Account);

        Assert.Equal(FeatureLockCodes.ToApiCodes(FeatureLockCodes.FreeTrialDefaults), locks.FeatureLock);
        Assert.Contains("report", locks.FeatureLock);
        Assert.Contains("create_transcation", locks.FeatureLock);
    }

    [Theory]
    [InlineData("Free")]
    [InlineData("Free Trial")]
    [InlineData("trial")]
    public async Task Feature_lock_applies_when_plan_name_is_free_or_trial(string planName)
    {
        _subs.Setup(r => r.GetActiveSubscriptionsByAccountIdAsync(TestIds.Account))
            .ReturnsAsync(new[]
            {
                new Subscription { Plan = new Plans { PlanName = planName } }
            });

        var locks = await _sut.GetFeatureLockAsync(TestIds.Account);
        Assert.NotEmpty(locks.FeatureLock);
    }

    [Fact]
    public async Task Paid_plan_has_no_feature_locks()
    {
        _subs.Setup(r => r.GetActiveSubscriptionsByAccountIdAsync(TestIds.Account))
            .ReturnsAsync(new[]
            {
                new Subscription { Plan = new Plans { PlanName = "Premium" } }
            });

        var locks = await _sut.GetFeatureLockAsync(TestIds.Account);
        Assert.Empty(locks.FeatureLock);
    }
}
