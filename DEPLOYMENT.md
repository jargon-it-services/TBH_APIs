# Deployment Guide - The Beauty Hub

## Option 1: EF Core Migrations (Recommended for Development)

### Advantages
- Version controlled database changes
- Easy rollback
- Automatic schema generation
- Built-in migration history

### Steps

1. **Build the solution**
   ```powershell
   cd E:\APP
   dotnet build TheBeautyHub.sln
   ```

2. **Create initial migration**
   ```powershell
   cd TheBeautyHubData
   dotnet ef migrations add InitialCreate --startup-project ../TheBeautyHubAPI
   ```

3. **Apply migration to database**
   ```powershell
   dotnet ef database update --startup-project ../TheBeautyHubAPI
   ```

4. **Deploy stored procedures**
   ```powershell
   sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i Scripts\AccountStoredProcedures.sql
   sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i Scripts\UserStoredProcedures.sql
   ```

## Option 2: Direct SQL Script (For Fresh Install)

### Advantages
- Complete control
- Single script deployment
- Includes stored procedures
- Faster for fresh installations

### Steps

1. **Run the complete setup script**
   ```powershell
   cd E:\APP
   sqlcmd -S localhost\SQLEXPRESS -i TheBeautyHubData\Scripts\CompleteSetup.sql
   ```

   This will:
   - Drop existing database (if any)
   - Create new database
   - Create all tables
   - Deploy all stored procedures

2. **Verify deployment**
   ```sql
   USE TheBeautyHubDb;
   
   -- Check tables
   SELECT * FROM INFORMATION_SCHEMA.TABLES;
   
   -- Check stored procedures
   SELECT name FROM sys.procedures ORDER BY name;
   ```

## Option 3: Production Deployment

### For Production/Staging Environments

1. **Generate SQL script from migration**
   ```powershell
   cd TheBeautyHubData
   dotnet ef migrations script --startup-project ../TheBeautyHubAPI --output ../Deploy/migration.sql
   ```

2. **Review the generated script** before executing on production

3. **Execute on production server**
   ```powershell
   sqlcmd -S PRODUCTION_SERVER -U sa -P password -i Deploy\migration.sql
   ```

4. **Deploy stored procedures**
   ```powershell
   sqlcmd -S PRODUCTION_SERVER -U sa -P password -d TheBeautyHubDb -i TheBeautyHubData\Scripts\AccountStoredProcedures.sql
   sqlcmd -S PRODUCTION_SERVER -U sa -P password -d TheBeautyHubDb -i TheBeautyHubData\Scripts\UserStoredProcedures.sql
   ```

## Connection String Configuration

### Development (appsettings.Development.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TheBeautyHubDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Production (appsettings.Production.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=PROD_SERVER;Database=TheBeautyHubDb;User Id=app_user;Password=SECURE_PASSWORD;Encrypt=True;TrustServerCertificate=False;MultipleActiveResultSets=true"
  }
}
```

### Azure SQL Database
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Database=TheBeautyHubDb;User ID=username@yourserver;Password=password;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

## Verification Steps

### 1. Check Database Creation
```sql
SELECT name, create_date FROM sys.databases WHERE name = 'TheBeautyHubDb';
```

### 2. Check Tables
```sql
USE TheBeautyHubDb;
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME;
```

Expected output:
- Account
- User
- __EFMigrationsHistory (if using EF migrations)

### 3. Check Stored Procedures
```sql
SELECT name FROM sys.procedures ORDER BY name;
```

Expected output:
- usp_Delete_Account
- usp_Delete_User
- usp_Get_AccountByCode
- usp_Get_AccountById
- usp_Get_AllAccounts
- usp_Get_AllUsers
- usp_Get_UserByEmail
- usp_Get_UserById
- usp_Get_UsersByAccountId
- usp_Get_UsersByManagerId
- usp_Insert_Account
- usp_Insert_User
- usp_Update_Account
- usp_Update_User
- usp_Update_UserPassword

### 4. Test Stored Procedure
```sql
-- Test account insert
EXEC usp_Insert_Account 
    @AccountCode = 'TEST001',
    @AccountName = 'Test Account',
    @AccountType = 'Customer',
    @Mode = 'one_time',
    @IsUnderTrial = 0;

-- Verify
SELECT * FROM Account WHERE AccountCode = 'TEST001';
```

## Rollback Procedures

### EF Core Migration Rollback
```powershell
# Rollback to specific migration
dotnet ef database update PreviousMigrationName --startup-project ../TheBeautyHubAPI

# Rollback to initial state
dotnet ef database update 0 --startup-project ../TheBeautyHubAPI
```

### Manual Rollback
```sql
-- Backup first!
BACKUP DATABASE TheBeautyHubDb TO DISK = 'C:\Backup\TheBeautyHubDb.bak';

-- Drop database
USE master;
DROP DATABASE TheBeautyHubDb;

-- Restore from backup
RESTORE DATABASE TheBeautyHubDb FROM DISK = 'C:\Backup\TheBeautyHubDb.bak';
```

## Database Backup Strategy

### Automated Backup Script
```sql
-- Create backup job
BACKUP DATABASE TheBeautyHubDb 
TO DISK = 'C:\Backup\TheBeautyHubDb_' + CONVERT(VARCHAR, GETDATE(), 112) + '.bak'
WITH FORMAT, COMPRESSION, STATS = 10;
```

### PowerShell Backup Script
```powershell
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupFile = "C:\Backup\TheBeautyHubDb_$timestamp.bak"

sqlcmd -S localhost\SQLEXPRESS -Q "BACKUP DATABASE TheBeautyHubDb TO DISK = '$backupFile' WITH FORMAT, COMPRESSION"
```

## Performance Optimization

### Add Indexes (if needed)
```sql
-- Already created in the setup script:
CREATE INDEX IX_User_AccountId ON [User](AccountId);
CREATE INDEX IX_User_ManagerId ON [User](ManagerId);
CREATE INDEX IX_User_Email ON [User](UserEmail) WHERE UserEmail IS NOT NULL;
CREATE INDEX IX_User_Mobile ON [User](UserMobile) WHERE UserMobile IS NOT NULL;
```

### Statistics Update
```sql
-- Update statistics for better query performance
UPDATE STATISTICS Account;
UPDATE STATISTICS [User];
```

## Monitoring

### Check Database Size
```sql
SELECT 
    DB_NAME() AS DatabaseName,
    SUM(size * 8 / 1024) AS SizeMB
FROM sys.database_files;
```

### Check Table Row Counts
```sql
SELECT 
    t.name AS TableName,
    SUM(p.rows) AS RowCount
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0, 1)
GROUP BY t.name
ORDER BY t.name;
```

## Troubleshooting

### Issue: Migration already applied
```powershell
# List applied migrations
dotnet ef migrations list --startup-project ../TheBeautyHubAPI

# Remove last migration (if not applied to DB yet)
dotnet ef migrations remove --startup-project ../TheBeautyHubAPI
```

### Issue: Database already exists
```sql
-- Option 1: Drop and recreate
USE master;
DROP DATABASE TheBeautyHubDb;

-- Option 2: Use the CompleteSetup.sql script which handles this
```

### Issue: Stored procedure already exists
```sql
-- Use ALTER PROCEDURE or CREATE OR ALTER PROCEDURE
-- The provided scripts use CREATE OR ALTER which handles this
```

## Security Checklist

- [ ] Use strong passwords for SQL authentication
- [ ] Enable SSL/TLS for database connections in production
- [ ] Implement least privilege access (don't use SA account)
- [ ] Regular backups configured
- [ ] Connection strings secured (use Azure Key Vault or similar)
- [ ] Database firewall rules configured
- [ ] Audit logging enabled

## Post-Deployment Testing

1. **Start the API**
   ```powershell
   cd TheBeautyHubAPI
   dotnet run
   ```

2. **Test endpoints via Swagger**
   - Open: https://localhost:7xxx/swagger
   - Create an account
   - Create a user
   - Test GET, PUT, DELETE operations

3. **Test database directly**
   ```sql
   -- Verify data
   SELECT COUNT(*) FROM Account;
   SELECT COUNT(*) FROM [User];
   ```

---

**Deployment Complete!** 

Your database is now ready to use with The Beauty Hub application.
