using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task<int> DeleteSubscriptionAsync(Guid subscriptionId)
        {
            var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
            if (subscription == null) return 0;
            
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<Subscription?> GetSubscriptionByIdAsync(Guid subscriptionId)
        {
            return await _context.Subscriptions
                .Where(s => s.SubscriptionId == subscriptionId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByAccountIdAsync(Guid accountId)
        {
            return await _context.Subscriptions
                .Where(s => s.AccountId == accountId)
                .ToListAsync();
        }
    }
}
