using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto createSubscriptionDto)
        {
            if (string.IsNullOrWhiteSpace(createSubscriptionDto.Status))
                throw new ArgumentException("Status is required.");

            ValidateAmounts(createSubscriptionDto.SubscriptionAmount, createSubscriptionDto.DiscountedAmount, 
                createSubscriptionDto.SubscriptionAmountAfterDiscount);

            var subscription = _mapper.Map<Subscription>(createSubscriptionDto);
            subscription.CreatedAt = DateTime.UtcNow;

            var insertedSubscription = await _subscriptionRepository.InsertSubscriptionAsync(subscription);
            return _mapper.Map<SubscriptionDto>(insertedSubscription);
        }

        public async Task<SubscriptionDto> UpdateSubscriptionAsync(UpdateSubscriptionDto updateSubscriptionDto)
        {
            if (string.IsNullOrWhiteSpace(updateSubscriptionDto.Status))
                throw new ArgumentException("Status is required.");

            ValidateAmounts(updateSubscriptionDto.SubscriptionAmount, updateSubscriptionDto.DiscountedAmount, 
                updateSubscriptionDto.SubscriptionAmountAfterDiscount);

            var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(updateSubscriptionDto.SubscriptionId);
            if (existingSubscription == null)
                throw new KeyNotFoundException($"Subscription with ID {updateSubscriptionDto.SubscriptionId} not found.");

            var subscription = _mapper.Map<Subscription>(updateSubscriptionDto);
            subscription.AccountId = existingSubscription.AccountId;
            subscription.PlanId = existingSubscription.PlanId;
            subscription.CreatedAt = existingSubscription.CreatedAt;
            subscription.CreatedBy = existingSubscription.CreatedBy;

            var updatedSubscription = await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
            return _mapper.Map<SubscriptionDto>(updatedSubscription);
        }

        public async Task<bool> DeleteSubscriptionAsync(Guid subscriptionId)
        {
            var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
            if (existingSubscription == null)
                throw new KeyNotFoundException($"Subscription with ID {subscriptionId} not found.");

            var result = await _subscriptionRepository.DeleteSubscriptionAsync(subscriptionId);
            return result > 0;
        }

        public async Task<SubscriptionDto?> GetSubscriptionByIdAsync(Guid subscriptionId)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
            return subscription == null ? null : _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync()
        {
            var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }

        public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionsByAccountIdAsync(Guid accountId)
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionsByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }

        public async Task<IEnumerable<SubscriptionDto>> GetActiveSubscriptionsByAccountIdAsync(Guid accountId)
        {
            var subscriptions = await _subscriptionRepository.GetActiveSubscriptionsByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }

        public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionsByPlanIdAsync(Guid planId)
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionsByPlanIdAsync(planId);
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }

        private void ValidateAmounts(decimal subscriptionAmount, decimal discountedAmount, decimal amountAfterDiscount)
        {
            if (subscriptionAmount < 0)
                throw new ArgumentException("Subscription amount cannot be negative.");

            if (discountedAmount < 0)
                throw new ArgumentException("Discounted amount cannot be negative.");

            if (amountAfterDiscount < 0)
                throw new ArgumentException("Amount after discount cannot be negative.");

            if (discountedAmount > subscriptionAmount)
                throw new ArgumentException("Discounted amount cannot exceed subscription amount.");
        }
    }
}
