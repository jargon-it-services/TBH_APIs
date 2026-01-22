using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Subscription repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface ISubscriptionRepository
    {
        Task<Subscription> InsertSubscriptionAsync(Subscription subscription);
        Task<Subscription> UpdateSubscriptionAsync(Subscription subscription);
        Task<int> DeleteSubscriptionAsync(Guid subscriptionId);
        Task<Subscription?> GetSubscriptionByIdAsync(Guid subscriptionId);
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
        Task<IEnumerable<Subscription>> GetSubscriptionsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Subscription>> GetActiveSubscriptionsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Subscription>> GetSubscriptionsByPlanIdAsync(Guid planId);
    }
}
