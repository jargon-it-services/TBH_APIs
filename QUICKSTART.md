# Quick Start Guide - The Beauty Hub

## Step-by-Step Setup (5 Minutes)

### Step 1: Verify Prerequisites ✓

Open PowerShell and verify installations:

```powershell
# Check .NET SDK
dotnet --version
# Should show 8.0.x or higher

# Check SQL Server
sqlcmd -S localhost\SQLEXPRESS -Q "SELECT @@VERSION"
# Should show SQL Server version
```

### Step 2: Configure Database Connection

1. Open [appsettings.json](TheBeautyHubAPI/appsettings.json)
2. Update connection string if needed:

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TheBeautyHubDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

**Common Configurations:**
- SQL Server Express: `Server=localhost\\SQLEXPRESS`
- LocalDB: `Server=(localdb)\\mssqllocaldb`
- Named Instance: `Server=localhost\\YOUR_INSTANCE`

### Step 3: Build the Solution

```powershell
cd E:\APP
dotnet build TheBeautyHub.sln
```

### Step 4: Create Database

```powershell
cd TheBeautyHubData
dotnet ef migrations add InitialCreate --startup-project ../TheBeautyHubAPI
dotnet ef database update --startup-project ../TheBeautyHubAPI
```

### Step 5: Deploy Stored Procedures

**Option A: Using SQL Command Line**

```powershell
cd E:\APP
sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i TheBeautyHubData\Scripts\AccountStoredProcedures.sql
sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i TheBeautyHubData\Scripts\UserStoredProcedures.sql
```

**Option B: Using SSMS or Azure Data Studio**

1. Connect to `localhost\SQLEXPRESS`
2. Select database: `TheBeautyHubDb`
3. Open and execute: `TheBeautyHubData\Scripts\AccountStoredProcedures.sql`
4. Open and execute: `TheBeautyHubData\Scripts\UserStoredProcedures.sql`

### Step 6: Run the API

```powershell
cd TheBeautyHubAPI
dotnet run
```

You should see:
```
Now listening on: https://localhost:7xxx
Now listening on: http://localhost:5xxx
```

### Step 7: Test with Swagger

1. Open browser to: `https://localhost:7xxx/swagger`
2. You'll see all API endpoints documented

### Step 8: Create Your First Account

In Swagger UI:

1. Click **POST /api/accounts**
2. Click **Try it out**
3. Use this JSON:

```json
{
  "accountCode": "ACC001",
  "accountName": "My Beauty Salon",
  "accountType": "FirmOwner",
  "mode": "subscription",
  "isUnderTrial": true,
  "trialStartedOn": "2026-01-17T00:00:00Z",
  "trialDuration": 30
}
```

4. Click **Execute**
5. Copy the `accountId` from the response

### Step 9: Create Your First User

1. Click **POST /api/users**
2. Click **Try it out**
3. Use this JSON (replace `accountId` with the one from Step 8):

```json
{
  "accountId": "YOUR-ACCOUNT-ID-HERE",
  "userRole": "Admin",
  "userName": "Admin User",
  "userEmail": "admin@beautyhub.com",
  "userMobile": "+1234567890",
  "password": "AdminPass123",
  "status": "Active"
}
```

4. Click **Execute**

## ✅ Success!

Your API is now running. You can:

- View all accounts: **GET /api/accounts**
- View all users: **GET /api/users**
- Get users by account: **GET /api/users/by-account/{accountId}**

## Common Issues & Fixes

### Issue: "A network-related or instance-specific error"

**Fix**: SQL Server is not running or connection string is wrong

```powershell
# Check SQL Server service
Get-Service | Where-Object {$_.DisplayName -like "*SQL*"}

# Start SQL Server if stopped
Start-Service MSSQL$SQLEXPRESS
```

### Issue: "Cannot find stored procedure"

**Fix**: Stored procedures not deployed

```powershell
# Redeploy stored procedures
sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i TheBeautyHubData\Scripts\AccountStoredProcedures.sql
sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i TheBeautyHubData\Scripts\UserStoredProcedures.sql
```

### Issue: "No migrations configuration type was found"

**Fix**: Run from correct directory

```powershell
cd E:\APP\TheBeautyHubData
dotnet ef migrations add InitialCreate --startup-project ../TheBeautyHubAPI
```

### Issue: Port already in use

**Fix**: Change port in launchSettings.json or kill existing process

```powershell
# Find process using port
Get-NetTCPConnection -LocalPort 7xxx | Select-Object -Property OwningProcess
Stop-Process -Id PROCESS_ID
```

## Useful Commands

```powershell
# Build solution
dotnet build

# Run API
cd TheBeautyHubAPI
dotnet run

# Run in watch mode (auto-reload on changes)
dotnet watch run

# Create new migration
cd TheBeautyHubData
dotnet ef migrations add MigrationName --startup-project ../TheBeautyHubAPI

# Update database
dotnet ef database update --startup-project ../TheBeautyHubAPI

# Rollback migration
dotnet ef database update PreviousMigrationName --startup-project ../TheBeautyHubAPI

# Remove last migration (if not applied)
dotnet ef migrations remove --startup-project ../TheBeautyHubAPI

# Drop database (careful!)
dotnet ef database drop --startup-project ../TheBeautyHubAPI
```

## Testing Endpoints with PowerShell

```powershell
# Get all accounts
Invoke-RestMethod -Uri "https://localhost:7xxx/api/accounts" -SkipCertificateCheck

# Create account
$body = @{
    accountCode = "ACC002"
    accountName = "Test Salon"
    accountType = "Customer"
    mode = "one_time"
    isUnderTrial = $false
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7xxx/api/accounts" `
    -Method Post `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

## Project Structure Quick Reference

```
TheBeautyHub/
├── TheBeautyHubData/           → Data layer (EF Core + Repositories)
│   ├── Entities/               → Database models
│   ├── Context/                → DbContext
│   ├── Repositories/           → Data access with stored procedures
│   └── Scripts/                → SQL stored procedures
│
├── TheBeautyHubCore/           → Business logic
│   ├── DTOs/                   → Data transfer objects
│   ├── Interfaces/             → Service contracts
│   └── Services/               → Business rules & validation
│
└── TheBeautyHubAPI/            → REST API
    ├── Controllers/            → API endpoints
    ├── Models/                 → Request/response models
    └── Program.cs              → Dependency injection setup
```

## Next: Adding Your Tables

See the main [README.md](README.md) for instructions on adding new tables to the system.

---

**Need Help?** Check the main README or review the code comments for detailed explanations.
