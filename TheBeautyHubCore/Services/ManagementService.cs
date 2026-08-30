using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Enums;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class ManagementService : IManagementService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IServicesRepository _servicesRepository;
        private readonly IExpensesTypeRepository _expensesTypeRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public ManagementService(
            IBranchRepository branchRepository,
            IStaffRepository staffRepository,
            IServicesRepository servicesRepository,
            IExpensesTypeRepository expensesTypeRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _branchRepository = branchRepository;
            _staffRepository = staffRepository;
            _servicesRepository = servicesRepository;
            _expensesTypeRepository = expensesTypeRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<AccountSummaryDto> GetAccountSummaryAsync(Guid accountId)
        {
            var branches = await _branchRepository.GetAllAsync(accountId);
            var staff = await _staffRepository.GetAllAsync(accountId);
            var services = await _servicesRepository.GetByAccountIdAsync(accountId);
            var expenses = await _expensesTypeRepository.GetByAccountIdAsync(accountId);
            var salaryRules = await _staffRepository.GetSalaryRulesAsync(accountId);

            return new AccountSummaryDto
            {
                TotalBranches = branches.Count(),
                TotalStaff = staff.Count,
                TotalServices = services.Count,
                TotalExpenses = expenses.Count,
                TotalSalaryRules = salaryRules.Count
            };
        }

        public async Task<FeatureLockDto> GetFeatureLockAsync(Guid accountId)
        {
            var subscriptions = await _subscriptionRepository.GetActiveSubscriptionsByAccountIdAsync(accountId);
            var planName = subscriptions
                .Select(s => s.Plan?.PlanName)
                .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name));

            var isFreeOrTrial = string.IsNullOrWhiteSpace(planName)
                || planName.Contains("free", StringComparison.OrdinalIgnoreCase)
                || planName.Contains("trial", StringComparison.OrdinalIgnoreCase);

            var locks = isFreeOrTrial
                ? FeatureLockCodes.ToApiCodes(FeatureLockCodes.FreeTrialDefaults).ToList()
                : new List<string>();

            return new FeatureLockDto { FeatureLock = locks };
        }
    }
}
