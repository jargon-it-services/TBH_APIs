using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto createSubscriptionDto);
        Task<SubscriptionDto> UpdateSubscriptionAsync(UpdateSubscriptionDto updateSubscriptionDto);
        Task<bool> DeleteSubscriptionAsync(Guid subscriptionId);
        Task<SubscriptionDto?> GetSubscriptionByIdAsync(Guid subscriptionId);
        Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync();
        Task<IEnumerable<SubscriptionDto>> GetSubscriptionsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<SubscriptionDto>> GetActiveSubscriptionsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<SubscriptionDto>> GetSubscriptionsByPlanIdAsync(Guid planId);
    }
}
