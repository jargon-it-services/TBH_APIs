using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHub.Tests.Services;

public class PlansServiceTests
{
    private readonly Mock<IPlansRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly PlansService _sut;

    public PlansServiceTests() => _sut = new PlansService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_requires_name()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreatePlanAsync(new CreatePlanDto { PlanName = " " }));
    }

    [Fact]
    public async Task Create_rejects_negative_cost()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.CreatePlanAsync(new CreatePlanDto { PlanName = "P", PlanCost = -1 }));
    }

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreatePlanDto { PlanName = "Premium", PlanCost = 99 };
        _mapper.Setup(m => m.Map<Plans>(dto)).Returns(new Plans { PlanName = "Premium" });
        _repo.Setup(r => r.InsertPlanAsync(It.IsAny<Plans>())).ReturnsAsync((Plans p) => p);
        _mapper.Setup(m => m.Map<PlansDto>(It.IsAny<Plans>())).Returns(new PlansDto { PlanName = "Premium" });
        Assert.Equal("Premium", (await _sut.CreatePlanAsync(dto)).PlanName);
    }

    [Fact]
    public async Task Update_missing_throws()
    {
        _repo.Setup(r => r.GetPlanByIdAsync(TestIds.Account)).ReturnsAsync((Plans?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdatePlanAsync(new UpdatePlanDto { PlanId = TestIds.Account, PlanName = "P" }));
    }

    [Fact]
    public async Task Delete_missing_throws()
    {
        _repo.Setup(r => r.GetPlanByIdAsync(TestIds.Account)).ReturnsAsync((Plans?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeletePlanAsync(TestIds.Account));
    }

    [Fact]
    public async Task Get_by_id_null_when_missing()
    {
        _repo.Setup(r => r.GetPlanByIdAsync(TestIds.Account)).ReturnsAsync((Plans?)null);
        Assert.Null(await _sut.GetPlanByIdAsync(TestIds.Account));
    }

    [Fact]
    public async Task List_and_active_map()
    {
        _repo.Setup(r => r.GetAllPlansAsync()).ReturnsAsync(Array.Empty<Plans>());
        _repo.Setup(r => r.GetActivePlansAsync()).ReturnsAsync(Array.Empty<Plans>());
        _mapper.Setup(m => m.Map<IEnumerable<PlansDto>>(It.IsAny<IEnumerable<Plans>>())).Returns(Array.Empty<PlansDto>());
        Assert.Empty(await _sut.GetAllPlansAsync());
        Assert.Empty(await _sut.GetActivePlansAsync());
    }
}

public class SubscriptionServiceTests
{
    private readonly Mock<ISubscriptionRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly SubscriptionService _sut;

    public SubscriptionServiceTests() => _sut = new SubscriptionService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_requires_status()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateSubscriptionAsync(new CreateSubscriptionDto { Status = " " }));
    }

    [Fact]
    public async Task Create_rejects_negative_amount()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateSubscriptionAsync(new CreateSubscriptionDto
        {
            Status = "Active",
            SubscriptionAmount = -1
        }));
    }

    [Fact]
    public async Task Create_rejects_discount_over_amount()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateSubscriptionAsync(new CreateSubscriptionDto
        {
            Status = "Active",
            SubscriptionAmount = 10,
            DiscountedAmount = 11
        }));
    }

    [Fact]
    public async Task Create_succeeds()
    {
        var dto = new CreateSubscriptionDto { Status = "Active", SubscriptionAmount = 10, DiscountedAmount = 1 };
        _mapper.Setup(m => m.Map<Subscription>(dto)).Returns(new Subscription());
        _repo.Setup(r => r.InsertSubscriptionAsync(It.IsAny<Subscription>())).ReturnsAsync(new Subscription());
        _mapper.Setup(m => m.Map<SubscriptionDto>(It.IsAny<Subscription>())).Returns(new SubscriptionDto { Status = "Active" });
        Assert.Equal("Active", (await _sut.CreateSubscriptionAsync(dto)).Status);
    }

    [Fact]
    public async Task Update_missing_throws()
    {
        _repo.Setup(r => r.GetSubscriptionByIdAsync(TestIds.Account)).ReturnsAsync((Subscription?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateSubscriptionAsync(new UpdateSubscriptionDto { SubscriptionId = TestIds.Account, Status = "Active" }));
    }

    [Fact]
    public async Task Queries_map()
    {
        _repo.Setup(r => r.GetAllSubscriptionsAsync()).ReturnsAsync(Array.Empty<Subscription>());
        _repo.Setup(r => r.GetSubscriptionsByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Subscription>());
        _repo.Setup(r => r.GetActiveSubscriptionsByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Subscription>());
        _repo.Setup(r => r.GetSubscriptionsByPlanIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Subscription>());
        _repo.Setup(r => r.GetSubscriptionByIdAsync(TestIds.Account)).ReturnsAsync((Subscription?)null);
        _mapper.Setup(m => m.Map<IEnumerable<SubscriptionDto>>(It.IsAny<IEnumerable<Subscription>>())).Returns(Array.Empty<SubscriptionDto>());
        Assert.Empty(await _sut.GetAllSubscriptionsAsync());
        Assert.Empty(await _sut.GetSubscriptionsByAccountIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetActiveSubscriptionsByAccountIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetSubscriptionsByPlanIdAsync(TestIds.Account));
        Assert.Null(await _sut.GetSubscriptionByIdAsync(TestIds.Account));
    }

    [Fact]
    public async Task Delete_missing_throws()
    {
        _repo.Setup(r => r.GetSubscriptionByIdAsync(TestIds.Account)).ReturnsAsync((Subscription?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteSubscriptionAsync(TestIds.Account));
    }
}

public class FirmServiceTests
{
    private readonly Mock<IFirmRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly FirmService _sut;

    public FirmServiceTests() => _sut = new FirmService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_requires_name() =>
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateFirmAsync(new CreateFirmDto { FirmName = "" }));

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateFirmDto { FirmName = "Salon", AccountId = TestIds.Account };
        _mapper.Setup(m => m.Map<Firm>(dto)).Returns(new Firm { FirmName = "Salon" });
        _repo.Setup(r => r.InsertFirmAsync(It.IsAny<Firm>())).ReturnsAsync((Firm f) => f);
        _mapper.Setup(m => m.Map<FirmDto>(It.IsAny<Firm>())).Returns(new FirmDto { FirmName = "Salon" });
        Assert.Equal("Salon", (await _sut.CreateFirmAsync(dto)).FirmName);
    }

    [Fact]
    public async Task Update_and_delete_missing_throw()
    {
        _repo.Setup(r => r.GetFirmByIdAsync(TestIds.Account)).ReturnsAsync((Firm?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateFirmAsync(new UpdateFirmDto { FirmId = TestIds.Account, FirmName = "X" }));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteFirmAsync(TestIds.Account));
    }

    [Fact]
    public async Task Gets_map()
    {
        _repo.Setup(r => r.GetFirmByIdAsync(TestIds.Account)).ReturnsAsync((Firm?)null);
        _repo.Setup(r => r.GetAllFirmsAsync()).ReturnsAsync(Array.Empty<Firm>());
        _repo.Setup(r => r.GetFirmsByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Firm>());
        _mapper.Setup(m => m.Map<IEnumerable<FirmDto>>(It.IsAny<IEnumerable<Firm>>())).Returns(Array.Empty<FirmDto>());
        Assert.Null(await _sut.GetFirmByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetAllFirmsAsync());
        Assert.Empty(await _sut.GetFirmsByAccountIdAsync(TestIds.Account));
    }
}

public class FirmDetailsServiceTests
{
    private readonly Mock<IFirmDetailsRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly FirmDetailsService _sut;

    public FirmDetailsServiceTests() => _sut = new FirmDetailsService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateFirmDetailsDto { AccountId = TestIds.Account, FirmId = TestIds.Branch, UserId = TestIds.User };
        _mapper.Setup(m => m.Map<FirmDetails>(dto)).Returns(new FirmDetails());
        _repo.Setup(r => r.InsertFirmDetailsAsync(It.IsAny<FirmDetails>())).ReturnsAsync(new FirmDetails());
        _mapper.Setup(m => m.Map<FirmDetailsDto>(It.IsAny<FirmDetails>())).Returns(new FirmDetailsDto { AccountId = TestIds.Account });
        Assert.Equal(TestIds.Account, (await _sut.CreateFirmDetailsAsync(dto)).AccountId);
    }

    [Fact]
    public async Task Update_and_delete_missing_throw()
    {
        _repo.Setup(r => r.GetFirmDetailsByIdAsync(TestIds.Account)).ReturnsAsync((FirmDetails?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateFirmDetailsAsync(new UpdateFirmDetailsDto { FirmDetailsId = TestIds.Account }));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteFirmDetailsAsync(TestIds.Account));
    }

    [Fact]
    public async Task Queries_map()
    {
        _repo.Setup(r => r.GetFirmDetailsByIdAsync(TestIds.Account)).ReturnsAsync((FirmDetails?)null);
        _repo.Setup(r => r.GetAllFirmDetailsAsync()).ReturnsAsync(Array.Empty<FirmDetails>());
        _repo.Setup(r => r.GetFirmDetailsByFirmIdAsync(TestIds.Branch)).ReturnsAsync(Array.Empty<FirmDetails>());
        _repo.Setup(r => r.GetFirmDetailsByUserIdAsync(TestIds.User)).ReturnsAsync(Array.Empty<FirmDetails>());
        _repo.Setup(r => r.GetFirmDetailsByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<FirmDetails>());
        _mapper.Setup(m => m.Map<IEnumerable<FirmDetailsDto>>(It.IsAny<IEnumerable<FirmDetails>>())).Returns(Array.Empty<FirmDetailsDto>());
        Assert.Null(await _sut.GetFirmDetailsByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetAllFirmDetailsAsync());
        Assert.Empty(await _sut.GetFirmDetailsByFirmIdAsync(TestIds.Branch));
        Assert.Empty(await _sut.GetFirmDetailsByUserIdAsync(TestIds.User));
        Assert.Empty(await _sut.GetFirmDetailsByAccountIdAsync(TestIds.Account));
    }
}

public class WalletServiceTests
{
    private readonly Mock<IWalletRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly WalletService _sut;

    public WalletServiceTests() => _sut = new WalletService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_rejects_null() =>
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateWalletAsync(null!));

    [Fact]
    public async Task Create_rejects_negative_amount() =>
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.CreateWalletAsync(new CreateWalletDto { Amount = -1, WalletType = "cash" }));

    [Fact]
    public async Task Create_requires_type() =>
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.CreateWalletAsync(new CreateWalletDto { Amount = 1, WalletType = " " }));

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateWalletDto { Amount = 10, WalletType = "cash", AccountId = TestIds.Account };
        _mapper.Setup(m => m.Map<Wallet>(dto)).Returns(new Wallet());
        _repo.Setup(r => r.InsertWalletAsync(It.IsAny<Wallet>())).ReturnsAsync(new Wallet());
        _mapper.Setup(m => m.Map<WalletDto>(It.IsAny<Wallet>())).Returns(new WalletDto { Amount = 10 });
        Assert.Equal(10, (await _sut.CreateWalletAsync(dto)).Amount);
    }

    [Fact]
    public async Task Update_and_delete_missing_throw()
    {
        _repo.Setup(r => r.GetWalletByIdAsync(TestIds.Account)).ReturnsAsync((Wallet?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateWalletAsync(new UpdateWalletDto { WalletId = TestIds.Account, Amount = 1, WalletType = "cash" }));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteWalletAsync(TestIds.Account));
    }

    [Fact]
    public async Task Gets_map()
    {
        _repo.Setup(r => r.GetWalletByIdAsync(TestIds.Account)).ReturnsAsync((Wallet?)null);
        _repo.Setup(r => r.GetWalletsByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Wallet>());
        _repo.Setup(r => r.GetAllWalletsAsync()).ReturnsAsync(Array.Empty<Wallet>());
        _mapper.Setup(m => m.Map<IEnumerable<WalletDto>>(It.IsAny<IEnumerable<Wallet>>())).Returns(Array.Empty<WalletDto>());
        Assert.Null(await _sut.GetWalletByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetWalletsByAccountIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetAllWalletsAsync());
    }
}

public class TransactionTypeServiceTests
{
    private readonly Mock<ITransactionTypeRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly TransactionTypeService _sut;

    public TransactionTypeServiceTests() => _sut = new TransactionTypeService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_rejects_null_and_empty_type()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateTransactionTypeAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.CreateTransactionTypeAsync(new CreateTransactionTypeDto { Type = " " }));
    }

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateTransactionTypeDto { Type = "sale" };
        _mapper.Setup(m => m.Map<TransactionType>(dto)).Returns(new TransactionType());
        _repo.Setup(r => r.InsertTransactionTypeAsync(It.IsAny<TransactionType>())).ReturnsAsync(new TransactionType());
        _mapper.Setup(m => m.Map<TransactionTypeDto>(It.IsAny<TransactionType>())).Returns(new TransactionTypeDto { Type = "sale" });
        Assert.Equal("sale", (await _sut.CreateTransactionTypeAsync(dto)).Type);
    }

    [Fact]
    public async Task Update_and_delete_missing_throw()
    {
        _repo.Setup(r => r.GetTransactionTypeByIdAsync(TestIds.Account)).ReturnsAsync((TransactionType?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateTransactionTypeAsync(new UpdateTransactionTypeDto { TransactionTypeId = TestIds.Account, Type = "sale" }));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteTransactionTypeAsync(TestIds.Account));
    }

    [Fact]
    public async Task Gets_map()
    {
        _repo.Setup(r => r.GetTransactionTypeByIdAsync(TestIds.Account)).ReturnsAsync((TransactionType?)null);
        _repo.Setup(r => r.GetAllTransactionTypesAsync()).ReturnsAsync(Array.Empty<TransactionType>());
        _repo.Setup(r => r.GetActiveTransactionTypesAsync()).ReturnsAsync(Array.Empty<TransactionType>());
        _mapper.Setup(m => m.Map<IEnumerable<TransactionTypeDto>>(It.IsAny<IEnumerable<TransactionType>>())).Returns(Array.Empty<TransactionTypeDto>());
        Assert.Null(await _sut.GetTransactionTypeByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetAllTransactionTypesAsync());
        Assert.Empty(await _sut.GetActiveTransactionTypesAsync());
    }
}

public class TransactionRulesServiceTests
{
    private readonly Mock<ITransactionRulesRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly TransactionRulesService _sut;

    public TransactionRulesServiceTests() => _sut = new TransactionRulesService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_rejects_null_and_empty_name()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateTransactionRulesAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.CreateTransactionRulesAsync(new CreateTransactionRulesDto { RuleName = " " }));
    }

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateTransactionRulesDto { RuleName = "R1" };
        _mapper.Setup(m => m.Map<TransactionRules>(dto)).Returns(new TransactionRules());
        _repo.Setup(r => r.InsertTransactionRulesAsync(It.IsAny<TransactionRules>())).ReturnsAsync(new TransactionRules());
        _mapper.Setup(m => m.Map<TransactionRulesDto>(It.IsAny<TransactionRules>())).Returns(new TransactionRulesDto { RuleName = "R1" });
        Assert.Equal("R1", (await _sut.CreateTransactionRulesAsync(dto)).RuleName);
    }

    [Fact]
    public async Task Update_and_delete_missing_throw()
    {
        _repo.Setup(r => r.GetTransactionRulesByIdAsync(TestIds.Account)).ReturnsAsync((TransactionRules?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateTransactionRulesAsync(new UpdateTransactionRulesDto { TransactionRuleId = TestIds.Account, RuleName = "R" }));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteTransactionRulesAsync(TestIds.Account));
    }

    [Fact]
    public async Task Gets_map()
    {
        _repo.Setup(r => r.GetTransactionRulesByIdAsync(TestIds.Account)).ReturnsAsync((TransactionRules?)null);
        _repo.Setup(r => r.GetTransactionRulesByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<TransactionRules>());
        _repo.Setup(r => r.GetAllTransactionRulesAsync()).ReturnsAsync(Array.Empty<TransactionRules>());
        _mapper.Setup(m => m.Map<IEnumerable<TransactionRulesDto>>(It.IsAny<IEnumerable<TransactionRules>>())).Returns(Array.Empty<TransactionRulesDto>());
        Assert.Null(await _sut.GetTransactionRulesByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetTransactionRulesByAccountIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetAllTransactionRulesAsync());
    }
}

public class TransactionDetailServiceTests
{
    private readonly Mock<ITransactionDetailRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly TransactionDetailService _sut;

    public TransactionDetailServiceTests() => _sut = new TransactionDetailService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateTransactionDetailDto { TransactionId = TestIds.Account, Amount = 10 };
        _mapper.Setup(m => m.Map<TransactionDetail>(dto)).Returns(new TransactionDetail());
        _repo.Setup(r => r.InsertAsync(It.IsAny<TransactionDetail>())).ReturnsAsync(new TransactionDetail());
        _mapper.Setup(m => m.Map<TransactionDetailDto>(It.IsAny<TransactionDetail>())).Returns(new TransactionDetailDto { Amount = 10 });
        Assert.Equal(10, (await _sut.CreateAsync(dto)).Amount);
    }

    [Fact]
    public async Task Update_missing_throws()
    {
        _repo.Setup(r => r.GetByIdAsync(TestIds.Account)).ReturnsAsync((TransactionDetail?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateAsync(TestIds.Account, new UpdateTransactionDetailDto()));
    }

    [Fact]
    public async Task Delete_and_gets()
    {
        _repo.Setup(r => r.DeleteAsync(TestIds.Account)).ReturnsAsync(1);
        _repo.Setup(r => r.GetByIdAsync(TestIds.Account)).ReturnsAsync((TransactionDetail?)null);
        _repo.Setup(r => r.GetByTransactionIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<TransactionDetail>());
        _mapper.Setup(m => m.Map<IEnumerable<TransactionDetailDto>>(It.IsAny<IEnumerable<TransactionDetail>>())).Returns(Array.Empty<TransactionDetailDto>());
        Assert.True(await _sut.DeleteAsync(TestIds.Account));
        Assert.Null(await _sut.GetByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetByTransactionIdAsync(TestIds.Account));
    }
}

public class ReportServiceTests
{
    private readonly Mock<IReportRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly ReportService _sut;

    public ReportServiceTests() => _sut = new ReportService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_requires_name() =>
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(new CreateReportDto { ReportName = " " }));

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateReportDto { ReportName = "Sales" };
        _mapper.Setup(m => m.Map<Report>(dto)).Returns(new Report());
        _repo.Setup(r => r.InsertAsync(It.IsAny<Report>())).ReturnsAsync(new Report());
        _mapper.Setup(m => m.Map<ReportDto>(It.IsAny<Report>())).Returns(new ReportDto { ReportName = "Sales" });
        Assert.Equal("Sales", (await _sut.CreateAsync(dto)).ReportName);
    }

    [Fact]
    public async Task Update_missing_throws()
    {
        _repo.Setup(r => r.GetByIdAsync(TestIds.Account)).ReturnsAsync((Report?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateAsync(TestIds.Account, new UpdateReportDto { ReportName = "X" }));
    }

    [Fact]
    public async Task Gets_and_delete()
    {
        _repo.Setup(r => r.DeleteAsync(TestIds.Account)).ReturnsAsync(0);
        _repo.Setup(r => r.GetByIdAsync(TestIds.Account)).ReturnsAsync((Report?)null);
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(Array.Empty<Report>());
        _repo.Setup(r => r.GetActiveReportsAsync()).ReturnsAsync(Array.Empty<Report>());
        _mapper.Setup(m => m.Map<IEnumerable<ReportDto>>(It.IsAny<IEnumerable<Report>>())).Returns(Array.Empty<ReportDto>());
        Assert.False(await _sut.DeleteAsync(TestIds.Account));
        Assert.Null(await _sut.GetByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetAllAsync());
        Assert.Empty(await _sut.GetActiveReportsAsync());
    }
}

public class ReportForAccountServiceTests
{
    private readonly Mock<IReportForAccountRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly ReportForAccountService _sut;

    public ReportForAccountServiceTests() => _sut = new ReportForAccountService(_repo.Object, _mapper.Object);

    [Fact]
    public async Task Create_inserts()
    {
        var dto = new CreateReportForAccountDto { AccountId = TestIds.Account, ReportId = TestIds.Service };
        _mapper.Setup(m => m.Map<ReportForAccount>(dto)).Returns(new ReportForAccount());
        _repo.Setup(r => r.InsertAsync(It.IsAny<ReportForAccount>())).ReturnsAsync(new ReportForAccount());
        _mapper.Setup(m => m.Map<ReportForAccountDto>(It.IsAny<ReportForAccount>())).Returns(new ReportForAccountDto { AccountId = TestIds.Account });
        Assert.Equal(TestIds.Account, (await _sut.CreateAsync(dto)).AccountId);
    }

    [Fact]
    public async Task Update_missing_throws()
    {
        _repo.Setup(r => r.GetByIdAsync(TestIds.Account)).ReturnsAsync((ReportForAccount?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.UpdateAsync(TestIds.Account, new UpdateReportForAccountDto { IsActive = true }));
    }

    [Fact]
    public async Task Gets_and_delete()
    {
        _repo.Setup(r => r.DeleteAsync(TestIds.Account)).ReturnsAsync(1);
        _repo.Setup(r => r.GetByIdAsync(TestIds.Account)).ReturnsAsync((ReportForAccount?)null);
        _repo.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<ReportForAccount>());
        _mapper.Setup(m => m.Map<IEnumerable<ReportForAccountDto>>(It.IsAny<IEnumerable<ReportForAccount>>())).Returns(Array.Empty<ReportForAccountDto>());
        Assert.True(await _sut.DeleteAsync(TestIds.Account));
        Assert.Null(await _sut.GetByIdAsync(TestIds.Account));
        Assert.Empty(await _sut.GetByAccountIdAsync(TestIds.Account));
    }
}
