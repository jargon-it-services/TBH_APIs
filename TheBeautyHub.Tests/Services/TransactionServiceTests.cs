using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _tx = new();
    private readonly Mock<IServicesRepository> _services = new();
    private readonly Mock<IExpensesTypeRepository> _expenses = new();
    private readonly Mock<IStaffRepository> _staff = new();
    private readonly Mock<IBranchRepository> _branches = new();
    private readonly TransactionService _sut;

    public TransactionServiceTests()
    {
        _sut = new TransactionService(_tx.Object, _services.Object, _expenses.Object, _staff.Object, _branches.Object);
    }

    private void StubSale()
    {
        _tx.Setup(r => r.GetByIdempotencyKeyAsync("idem-1", TestIds.Account)).ReturnsAsync((Transaction?)null);
        _tx.Setup(r => r.CountByAccountAsync(TestIds.Account)).ReturnsAsync(0);
        _branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        _services.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync(Fixtures.ServiceEntity());
        _tx.Setup(r => r.InsertAsync(It.IsAny<Transaction>()))
            .ReturnsAsync((Transaction t) =>
            {
                t.TransactionId = Guid.NewGuid();
                return t;
            });
        _tx.Setup(r => r.GetDetailsAsync(It.IsAny<Guid>(), TestIds.Account))
            .ReturnsAsync((Guid id, Guid _) => new Transaction
            {
                TransactionId = id,
                Code = "TXN1",
                AccountId = TestIds.Account,
                Status = TransactionStatus.Paid.ToApiValue(),
                TotalAmount = 120,
                EditableUntil = DateTime.UtcNow.AddHours(2)
            });
    }

    [Fact]
    public async Task Create_sale_with_upi_succeeds()
    {
        StubSale();
        var saved = await _sut.CreateAsync(Fixtures.Sale());
        Assert.Equal("TXN1", saved.Id);
        Assert.Equal(TransactionStatus.Paid.ToApiValue(), saved.Status);
    }

    [Fact]
    public async Task Create_returns_existing_when_idempotency_matches()
    {
        var existing = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Code = "TXN9",
            AccountId = TestIds.Account,
            Status = TransactionStatus.Paid.ToApiValue(),
            TotalAmount = 10,
            EditableUntil = DateTime.UtcNow.AddHours(1)
        };
        _tx.Setup(r => r.GetByIdempotencyKeyAsync("idem-1", TestIds.Account)).ReturnsAsync(existing);

        var saved = await _sut.CreateAsync(Fixtures.Sale());
        Assert.Equal("TXN9", saved.Id);
        _tx.Verify(r => r.InsertAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task Create_requires_idempotency()
    {
        var dto = Fixtures.Sale();
        dto.IdempotencyKey = null;
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionIdempotencyRequired, ex.Message);
    }

    [Theory]
    [InlineData("refund")]
    [InlineData("")]
    public async Task Create_rejects_invalid_type(string type)
    {
        var dto = Fixtures.Sale();
        dto.Type = type;
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
    }

    [Fact]
    public async Task Create_rejects_unknown_payment_mode()
    {
        var dto = Fixtures.Sale();
        dto.PaymentMode = "bitcoin";
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionPaymentModeInvalid, ex.Message);
    }

    [Fact]
    public async Task Create_requires_line_items()
    {
        var dto = Fixtures.Sale();
        dto.Services = new List<SaveTransactionLineDto>();
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionServicesRequired, ex.Message);
    }

    [Fact]
    public async Task Create_expense_uses_expense_catalog()
    {
        var dto = Fixtures.Sale();
        dto.Type = "expense";
        dto.Services = new List<SaveTransactionLineDto>
        {
            new() { ServiceId = TestIds.Expense, Quantity = 1 }
        };
        _tx.Setup(r => r.GetByIdempotencyKeyAsync("idem-1", TestIds.Account)).ReturnsAsync((Transaction?)null);
        _tx.Setup(r => r.CountByAccountAsync(TestIds.Account)).ReturnsAsync(1);
        _branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        _expenses.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync(Fixtures.ExpenseEntity());
        _tx.Setup(r => r.InsertAsync(It.IsAny<Transaction>()))
            .ReturnsAsync((Transaction t) => { t.TransactionId = Guid.NewGuid(); return t; });
        _tx.Setup(r => r.GetDetailsAsync(It.IsAny<Guid>(), TestIds.Account))
            .ReturnsAsync((Guid id, Guid _) => new Transaction
            {
                TransactionId = id,
                Code = "TXN2",
                Type = TransactionKind.Expense.ToApiValue(),
                Status = TransactionStatus.Paid.ToApiValue(),
                AccountId = TestIds.Account
            });

        var saved = await _sut.CreateAsync(dto);
        Assert.Equal("TXN2", saved.Id);
        _services.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Mark_paid_updates_status()
    {
        var existing = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Code = "TXN1",
            AccountId = TestIds.Account,
            Status = TransactionStatus.Pending.ToApiValue()
        };
        _tx.Setup(r => r.GetDetailsAsync(existing.TransactionId, TestIds.Account)).ReturnsAsync(existing);
        _tx.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

        var saved = await _sut.MarkPaidAsync(existing.TransactionId.ToString(), TestIds.Account);
        Assert.Equal(TransactionStatus.Paid.ToApiValue(), existing.Status);
        Assert.NotNull(saved.PaidAt);
    }

    [Fact]
    public async Task List_exposes_enum_filter_values()
    {
        _tx.Setup(r => r.GetListByAccountAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Transaction>());
        _services.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<TheBeautyHubData.Entities.Services>());
        _staff.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Staff>());
        _branches.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(Array.Empty<Branch>());

        var list = await _sut.GetListAsync(TestIds.Account);

        Assert.Contains("sale", list.Filters.Types);
        Assert.Contains("expense", list.Filters.Types);
        Assert.Contains("cash", list.Filters.PaymentModes);
        Assert.Contains("paid", list.Filters.Statuses);
        Assert.Equal("INR", list.Filters.Currency);
    }

    [Fact]
    public async Task Bootstrap_uses_staff_branch()
    {
        _services.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.ServiceEntity() });
        _expenses.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.ExpenseEntity() });
        _staff.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.StaffEntity() });
        _branches.Setup(r => r.GetAllAsync(TestIds.Account)).ReturnsAsync(new[] { Fixtures.BranchEntity() });
        _tx.Setup(r => r.GetServiceUsageCountsAsync(TestIds.Account)).ReturnsAsync(new Dictionary<Guid, int>());
        _tx.Setup(r => r.GetLatestByUserAsync(TestIds.Account, TestIds.User)).ReturnsAsync((Transaction?)null);
        _staff.Setup(r => r.GetByUserIdAsync(TestIds.User, TestIds.Account)).ReturnsAsync(Fixtures.StaffEntity());

        var boot = await _sut.GetBootstrapAsync(TestIds.Account, TestIds.User, new[] { "Owner" });
        Assert.Equal(TestIds.Branch, boot.LoggedInBranchId);
        Assert.Single(boot.Services);
    }

    [Fact]
    public async Task Create_rejects_unknown_branch()
    {
        var dto = Fixtures.Sale();
        _tx.Setup(r => r.GetByIdempotencyKeyAsync("idem-1", TestIds.Account)).ReturnsAsync((Transaction?)null);
        _branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync((Branch?)null);
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionBranchInvalid, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_unknown_service()
    {
        StubSale();
        _services.Setup(r => r.GetByIdAsync(TestIds.Service, TestIds.Account)).ReturnsAsync((TheBeautyHubData.Entities.Services?)null);
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(Fixtures.Sale()));
        Assert.Equal(ApiMessages.TransactionInvalidServiceIds, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_zero_quantity()
    {
        StubSale();
        var dto = Fixtures.Sale();
        dto.Services![0].Quantity = 0;
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionQuantityInvalid, ex.Message);
    }

    [Fact]
    public async Task Create_rejects_missing_line_service_id()
    {
        StubSale();
        var dto = Fixtures.Sale();
        dto.Services![0].ServiceId = null;
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionLineServiceRequired, ex.Message);
    }

    [Theory]
    [InlineData("cash")]
    [InlineData("UPI")]
    [InlineData("card")]
    public async Task Create_accepts_payment_modes(string mode)
    {
        StubSale();
        var dto = Fixtures.Sale();
        dto.PaymentMode = mode;
        var saved = await _sut.CreateAsync(dto);
        Assert.Equal("TXN1", saved.Id);
    }

    [Fact]
    public async Task Get_details_by_code()
    {
        _tx.Setup(r => r.GetByCodeAsync("TXN1", TestIds.Account)).ReturnsAsync(new Transaction
        {
            Code = "TXN1",
            AccountId = TestIds.Account,
            Status = TransactionStatus.Paid.ToApiValue(),
            TotalAmount = 120
        });
        var detail = await _sut.GetDetailsAsync("TXN1", TestIds.Account);
        Assert.NotNull(detail);
        Assert.Equal("TXN1", detail!.Id);
    }

    [Fact]
    public async Task Get_details_missing_is_null()
    {
        _tx.Setup(r => r.GetDetailsAsync(TestIds.Service, TestIds.Account)).ReturnsAsync((Transaction?)null);
        Assert.Null(await _sut.GetDetailsAsync(TestIds.Service.ToString(), TestIds.Account));
    }

    [Fact]
    public async Task Update_outside_edit_window_fails()
    {
        var existing = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Code = "TXN1",
            AccountId = TestIds.Account,
            EditableUntil = DateTime.UtcNow.AddMinutes(-1)
        };
        _tx.Setup(r => r.GetDetailsAsync(existing.TransactionId, TestIds.Account)).ReturnsAsync(existing);
        var dto = Fixtures.Sale();
        dto.IdempotencyKey = null;
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.UpdateAsync(existing.TransactionId.ToString(), dto));
        Assert.Equal(TransactionService.EditWindowClosedCode, ex.Message);
    }

    [Fact]
    public async Task Mark_paid_missing_throws()
    {
        _tx.Setup(r => r.GetDetailsAsync(TestIds.Service, TestIds.Account)).ReturnsAsync((Transaction?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.MarkPaidAsync(TestIds.Service.ToString(), TestIds.Account));
    }

    [Fact]
    public async Task Create_unknown_expense_fails()
    {
        var dto = Fixtures.Sale();
        dto.Type = "expense";
        dto.Services = new List<SaveTransactionLineDto> { new() { ServiceId = TestIds.Expense, Quantity = 1 } };
        _tx.Setup(r => r.GetByIdempotencyKeyAsync("idem-1", TestIds.Account)).ReturnsAsync((Transaction?)null);
        _branches.Setup(r => r.GetByIdAsync(TestIds.Branch)).ReturnsAsync(Fixtures.BranchEntity());
        _expenses.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync((ExpensesType?)null);
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionInvalidExpenseIds, ex.Message);
    }

    [Fact]
    public async Task Create_empty_type_is_required()
    {
        var dto = Fixtures.Sale();
        dto.Type = " ";
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(dto));
        Assert.Equal(ApiMessages.TransactionTypeRequired, ex.Message);
    }
}
