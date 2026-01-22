using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubData.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly BeautyHubDbContext _context;

        public SubscriptionRepository(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription> InsertSubscriptionAsync(Subscription subscription)
        {
            var parameters = new[]
            {
                new SqlParameter("@AccountId", subscription.AccountId),
                new SqlParameter("@PlanId", subscription.PlanId),
                new SqlParameter("@Status", subscription.Status),
                new SqlParameter("@ExpiryOn", (object?)subscription.ExpiryOn ?? DBNull.Value),
                new SqlParameter("@CreatedBy", (object?)subscription.CreatedBy ?? DBNull.Value),
                new SqlParameter("@SubscriptionAmount", subscription.SubscriptionAmount),
                new SqlParameter("@DiscountedAmount", subscription.DiscountedAmount),
                new SqlParameter("@SubscriptionAmountAfterDiscount", subscription.SubscriptionAmountAfterDiscount),
                new SqlParameter("@DiscountType", (object?)subscription.DiscountType ?? DBNull.Value)
            };

            var result = await _context.Subscriptions
                .FromSqlRaw("EXEC usp_Insert_Subscription @AccountId, @PlanId, @Status, @ExpiryOn, @CreatedBy, @SubscriptionAmount, @DiscountedAmount, @SubscriptionAmountAfterDiscount, @DiscountType", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? subscription;
        }

        public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription)
        {
            var parameters = new[]
            {
                new SqlParameter("@SubscriptionId", subscription.SubscriptionId),
                new SqlParameter("@Status", subscription.Status),
                new SqlParameter("@ExpiryOn", (object?)subscription.ExpiryOn ?? DBNull.Value),
                new SqlParameter("@SubscriptionAmount", subscription.SubscriptionAmount),
                new SqlParameter("@DiscountedAmount", subscription.DiscountedAmount),
                new SqlParameter("@SubscriptionAmountAfterDiscount", subscription.SubscriptionAmountAfterDiscount),
                new SqlParameter("@DiscountType", (object?)subscription.DiscountType ?? DBNull.Value)
            };

            var result = await _context.Subscriptions
                .FromSqlRaw("EXEC usp_Update_Subscription @SubscriptionId, @Status, @ExpiryOn, @SubscriptionAmount, @DiscountedAmount, @SubscriptionAmountAfterDiscount, @DiscountType", parameters)
                .ToListAsync();

            return result.FirstOrDefault() ?? subscription;
        }

        public async Task<int> DeleteSubscriptionAsync(Guid subscriptionId)
        {
            var parameter = new SqlParameter("@SubscriptionId", subscriptionId);
            return await _context.Database
                .ExecuteSqlRawAsync("EXEC usp_Delete_Subscription @SubscriptionId", parameter);
        }

        public async Task<Subscription?> GetSubscriptionByIdAsync(Guid subscriptionId)
        {
            var parameter = new SqlParameter("@SubscriptionId", subscriptionId);
            var result = await _context.Subscriptions
                .FromSqlRaw("EXEC usp_Get_SubscriptionById @SubscriptionId", parameter)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _context.Subscriptions
                .FromSqlRaw("EXEC usp_Get_AllSubscriptions")
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.Subscriptions
                .FromSqlRaw("EXEC usp_Get_SubscriptionsByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetActiveSubscriptionsByAccountIdAsync(Guid accountId)
        {
            var parameter = new SqlParameter("@AccountId", accountId);
            return await _context.Subscriptions
                .FromSqlRaw("EXEC usp_Get_ActiveSubscriptionsByAccountId @AccountId", parameter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByPlanIdAsync(Guid planId)
        {
            var parameter = new SqlParameter("@PlanId", planId);
            return await _context.Subscriptions
                .FromSqlRaw("EXEC usp_Get_SubscriptionsByPlanId @PlanId", parameter)
                .ToListAsync();
        }
    }
}
