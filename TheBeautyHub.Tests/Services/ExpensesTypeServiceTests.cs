using TheBeautyHubCore.Constants;
using TheBeautyHubCore.Services;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Enums;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHub.Tests.TestData;

namespace TheBeautyHub.Tests.Services;

public class ExpensesTypeServiceTests
{
    private readonly Mock<IExpensesTypeRepository> _repo = new();
    private readonly ExpensesTypeService _sut;

    public ExpensesTypeServiceTests() => _sut = new ExpensesTypeService(_repo.Object);

    private void StubLookups()
    {
        _repo.Setup(r => r.GetBranchesByIdsAsync(TestIds.Account, It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync((Guid _, IEnumerable<Guid> ids) =>
                ids.Select(id => new Branch { BranchId = id, AccountId = TestIds.Account, Name = "B" }).ToList());
        _repo.Setup(r => r.InsertAsync(It.IsAny<ExpensesType>()))
            .ReturnsAsync((ExpensesType e) => { e.ExpensesTypeId = TestIds.Expense; return e; });
        _repo.Setup(r => r.ReplaceBranchesAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
            .Returns(Task.CompletedTask);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<ExpensesType>())).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Update_uses_selected_branch_when_not_all_branches()
    {
        StubLookups();
        var existing = Fixtures.ExpenseEntity();
        _repo.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync(existing);

        await _sut.UpdateAsync(TestIds.Expense, Fixtures.ExpenseUpdate());

        _repo.Verify(r => r.ReplaceBranchesAsync(TestIds.Expense, It.Is<IEnumerable<Guid>>(ids => ids.Single() == TestIds.OtherBranch)));
        Assert.Equal(RecordStatus.Active.ToApiValue(), existing.Status);
        Assert.False(existing.AllBranches);
    }

    [Fact]
    public async Task Update_without_branches_when_not_all_fails()
    {
        var dto = Fixtures.ExpenseUpdate();
        dto.Branches = new List<Guid>();
        _repo.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync(Fixtures.ExpenseEntity());
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateAsync(TestIds.Expense, dto));
        Assert.Equal(ApiMessages.BranchesRequiredWhenNotAll, ex.Message);
    }

    [Fact]
    public async Task Create_all_branches_does_not_require_ids()
    {
        StubLookups();
        var dto = Fixtures.ExpenseUpdate();
        dto.AllBranches = true;
        dto.Branches = null;
        await _sut.CreateAsync(dto);
        _repo.Verify(r => r.GetBranchesByIdsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()), Times.Never);
    }

    [Fact]
    public async Task Update_status_does_not_clear_name()
    {
        var existing = Fixtures.ExpenseEntity();
        _repo.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync(existing);
        _repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

        await _sut.UpdateStatusAsync(TestIds.Expense, TestIds.Account, "Inactive");

        Assert.Equal("Water Bottle", existing.ExpensesTypeName);
        Assert.Equal(RecordStatus.Inactive.ToApiValue(), existing.Status);
    }

    [Fact]
    public async Task List_and_details_empty_account()
    {
        _repo.Setup(r => r.GetByAccountIdAsync(TestIds.Account)).ReturnsAsync(Array.Empty<ExpensesType>());
        _repo.Setup(r => r.GetDetailsByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync((ExpensesType?)null);
        Assert.Empty(await _sut.GetListAsync(TestIds.Account));
        Assert.Null(await _sut.GetDetailsAsync(TestIds.Expense, TestIds.Account));
    }

    [Fact]
    public async Task Delete_missing_throws()
    {
        _repo.Setup(r => r.GetByIdAsync(TestIds.Expense, TestIds.Account)).ReturnsAsync((ExpensesType?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteAsync(TestIds.Expense, TestIds.Account));
    }
}
