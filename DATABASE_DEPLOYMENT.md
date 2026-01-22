# Production Database Deployment Guide

## ✅ Single-Command Deployment

The entire database setup (tables + stored procedures) is fully automated through EF Core migrations.

### Quick Start

```bash
# Single command deploys everything
dotnet ef database update --project TheBeautyHubData/TheBeautyHubData.csproj --startup-project TheBeautyHubAPI/TheBeautyHubAPI.csproj
```

### What Gets Deployed

This command creates:
- ✅ **18 Tables** with all constraints
- ✅ **117 Stored Procedures** (CRUD operations)
- ✅ **30 Foreign Keys**
- ✅ **53 Indexes** (including unique constraints)

### Database Objects Created

| Category | Count | Details |
|----------|-------|---------|
| Tables | 18 | Account, User, Firm, FirmDetails, Plans, Subscription, Wallet, ExpensesType, Services, TransactionType, TransactionRules, Transactions, TransactionsDetails, Reports, ReportsForAccount, Partner, UserSessions, ExceptionLogs |
| Stored Procedures | 117 | Complete CRUD operations for all entities |
| Foreign Keys | 30 | All relationships enforced |
| Indexes | 53 | Performance optimized |

### Production Configuration

1. **Update Connection String** in `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=PROD_SERVER;Database=TheBeautyHub;User Id=app_user;Password=***;TrustServerCertificate=True;MultipleActiveResultSets=true;Command Timeout=120"
  }
}
```

2. **Deploy to Production**:

```bash
# Option 1: Direct migration (recommended for initial setup)
dotnet ef database update --project TheBeautyHubData/TheBeautyHubData.csproj --startup-project TheBeautyHubAPI/TheBeautyHubAPI.csproj

# Option 2: Generate SQL script for manual review/execution
dotnet ef migrations script --project TheBeautyHubData/TheBeautyHubData.csproj --startup-project TheBeautyHubAPI/TheBeautyHubAPI.csproj --output deployment.sql
```

### CI/CD Integration Examples

**Azure DevOps:**
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Database Migration'
  inputs:
    command: 'custom'
    custom: 'ef'
    arguments: 'database update --project TheBeautyHubData/TheBeautyHubData.csproj --startup-project TheBeautyHubAPI/TheBeautyHubAPI.csproj'
```

**GitHub Actions:**
```yaml
- name: Apply Database Migrations
  run: dotnet ef database update --project TheBeautyHubData/TheBeautyHubData.csproj --startup-project TheBeautyHubAPI/TheBeautyHubAPI.csproj
  env:
    ConnectionStrings__DefaultConnection: ${{ secrets.DB_CONNECTION_STRING }}
```

### Verification

```bash
sqlcmd -S YOUR_SERVER -d TheBeautyHub -Q "SELECT 'Tables' as Category, COUNT(*) as Count FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME != '__EFMigrationsHistory' UNION ALL SELECT 'Stored Procedures', COUNT(*) FROM sys.procedures"
```

Expected: **18 tables, 117 stored procedures**

### Rollback

```bash
# List migrations
dotnet ef migrations list --project TheBeautyHubData/TheBeautyHubData.csproj

# Rollback to previous migration
dotnet ef database update <PreviousMigrationName> --project TheBeautyHubData/TheBeautyHubData.csproj
```

### Best Practices

1. ✅ Backup database before deployment
2. ✅ Test in staging environment first
3. ✅ Use SQL authentication with minimal permissions
4. ✅ Review generated SQL scripts for sensitive deployments
5. ✅ Monitor migration execution time

### Troubleshooting

**Permission errors**: Grant CREATE DATABASE, CREATE TABLE, CREATE PROCEDURE permissions

**Timeout errors**: Increase `Command Timeout` in connection string to 180 seconds

**File not found errors**: Ensure Scripts folder is included in project and copied to output directory

---

## How It Works

The migration file (`20260118085152_InitialCreate.cs`) includes:

1. Table creation via EF Core
2. Automatic execution of all SQL scripts from `TheBeautyHubData/Scripts/` folder
3. Stored procedures are parsed and executed during migration

This ensures **100% consistency** between development and production environments.
