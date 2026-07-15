# Script to generate EF Core-based repositories for PostgreSQL

$repositories = @(
    @{
        Name = 'FirmRepository'
        Entity = 'Firm'
        DbSet = 'Firms'
        IdProperty = 'FirmId'
        Methods = @('InsertFirmAsync', 'UpdateFirmAsync', 'DeleteFirmAsync', 'GetFirmByIdAsync', 'GetAllFirmsAsync', 'GetFirmsByAccountIdAsync')
    },
    @{
        Name = 'SubscriptionRepository'
        Entity = 'Subscription'
        DbSet = 'Subscriptions'
        IdProperty = 'SubscriptionId'
        Methods = @('InsertSubscriptionAsync', 'UpdateSubscriptionAsync', 'DeleteSubscriptionAsync', 'GetSubscriptionByIdAsync', 'GetAllSubscriptionsAsync', 'GetSubscriptionsByAccountIdAsync')
    },
    @{
        Name = 'WalletRepository'
        Entity = 'Wallet'
        DbSet = 'Wallets'
        IdProperty = 'WalletId'
        Methods = @('InsertWalletAsync', 'UpdateWalletAsync', 'DeleteWalletAsync', 'GetWalletByIdAsync', 'GetAllWalletsAsync', 'GetWalletsByAccountIdAsync')
    },
    @{
        Name = 'PlansRepository'
        Entity = 'Plans'
        DbSet = 'Plans'
        IdProperty = 'PlanId'
        Methods = @('InsertPlanAsync', 'UpdatePlanAsync', 'DeletePlanAsync', 'GetPlanByIdAsync', 'GetAllPlansAsync', 'GetActivePlansAsync')
    },
    @{
        Name = 'ServicesRepository'
        Entity = 'Services'
        DbSet = 'Services'
        IdProperty = 'ServiceId'
        Methods = @('InsertServiceAsync', 'UpdateServiceAsync', 'DeleteServiceAsync', 'GetServiceByIdAsync', 'GetAllServicesAsync', 'GetServicesByAccountIdAsync')
    },
    @{
        Name = 'ExpensesTypeRepository'
        Entity = 'ExpensesType'
        DbSet = 'ExpensesTypes'
        IdProperty = 'ExpensesTypeId'
        Methods = @('InsertExpensesTypeAsync', 'UpdateExpensesTypeAsync', 'DeleteExpensesTypeAsync', 'GetExpensesTypeByIdAsync', 'GetAllExpensesTypesAsync', 'GetExpensesTypesByAccountIdAsync')
    }
)

foreach ($repo in $repositories) {
    $content = @"
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
    public class $($repo.Name) : I$($repo.Name)
    {
        private readonly BeautyHubDbContext _context;

        public $($repo.Name)(BeautyHubDbContext context)
        {
            _context = context;
        }

        public async Task<$($repo.Entity)> $($repo.Methods[0])($($repo.Entity) entity)
        {
            _context.$($repo.DbSet).Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<$($repo.Entity)> $($repo.Methods[1])($($repo.Entity) entity)
        {
            _context.$($repo.DbSet).Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> $($repo.Methods[2])(Guid id)
        {
            var entity = await _context.$($repo.DbSet).FindAsync(id);
            if (entity == null) return 0;
            
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<$($repo.Entity)?> $($repo.Methods[3])(Guid id)
        {
            return await _context.$($repo.DbSet)
                .Where(e => e.$($repo.IdProperty) == id && !e.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<$($repo.Entity)>> $($repo.Methods[4])()
        {
            return await _context.$($repo.DbSet)
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }
    }
}
"@
    
    $filePath = "TheBeautyHubData\Repositories\$($repo.Name).cs"
    Set-Content -Path $filePath -Value $content
    Write-Host "Created $filePath"
}

Write-Host "Repository generation complete!"
