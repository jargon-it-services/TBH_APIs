# The Beauty Hub Solution

## Overview
The Beauty Hub is a comprehensive three-tier ASP.NET Core application designed for managing beauty salon accounts and users. It follows clean architecture principles with a clear separation of concerns across three projects:

1. **TheBeautyHubData** - Data Access Layer (EF Core + Stored Procedures)
2. **TheBeautyHubCore** - Business Logic Layer (Services + Validation)
3. **TheBeautyHubAPI** - Presentation Layer (REST API + Controllers)

## Architecture

### Project Structure
```
TheBeautyHub/
├── TheBeautyHubData/           # Data Access Layer
│   ├── Context/                # DbContext
│   ├── Entities/               # Entity classes (Account, User)
│   ├── Repositories/           # Repository implementations
│   └── Scripts/                # SQL Stored Procedures
│
├── TheBeautyHubCore/           # Business Logic Layer
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Interfaces/             # Service interfaces
│   └── Services/               # Business logic implementations
│
└── TheBeautyHubAPI/            # Presentation Layer
    ├── Controllers/            # API Controllers
    ├── Models/                 # Request/Response models
    └── MappingProfile.cs       # AutoMapper configuration
```

## Database Schema

### Account Table
- **AccountId**: UNIQUEIDENTIFIER (PK)
- **AccountCode**: VARCHAR(12) (Unique, 6+ chars)
- **AccountName**: NVARCHAR(200)
- **AccountType**: VARCHAR(20) (FirmOwner | Customer)
- **Mode**: VARCHAR(20) (subscription | one_time)
- **IsUnderTrial**: BIT
- **TrialStartedOn**: DATETIME2(7)
- **TrialDuration**: INT (days)
- **TrialExpiredOn**: DATETIME2(7)
- **CreatedBy**: UNIQUEIDENTIFIER
- **CreatedAt**: DATETIME2(7)
- **LastUpdated**: DATETIME2(7)
- **IsDeleted**: BIT (Soft delete)

### User Table
- **UserId**: UNIQUEIDENTIFIER (PK)
- **AccountId**: UNIQUEIDENTIFIER (FK → Account)
- **UserRole**: VARCHAR(20) (Admin | Manager | Employee)
- **UserName**: NVARCHAR(150)
- **UserEmail**: NVARCHAR(256) (Unique)
- **UserMobile**: NVARCHAR(20) (Unique)
- **UserPasswordHash**: VARBINARY(64)
- **EmailVerified**: BIT
- **MobileVerified**: BIT
- **WorkerPaymentType**: VARCHAR(30) (Fix Pay | FP + Incentive | Incentive)
- **ManagerId**: UNIQUEIDENTIFIER (FK → User, self-referencing)
- **CreatedBy**: UNIQUEIDENTIFIER
- **CreatedAt**: DATETIME2(7)
- **LastUpdated**: DATETIME2(7)
- **IsDeleted**: BIT (Soft delete)
- **Status**: VARCHAR(20) (default: "Active")

## Prerequisites

- .NET 8.0 SDK or later
- SQL Server Express (or SQL Server)
- Visual Studio 2022 or VS Code

## Initial Setup

### 1. Update Connection String

Edit [appsettings.json](TheBeautyHubAPI/appsettings.json):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TheBeautyHubDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Adjust the connection string based on your SQL Server instance:**
- For SQL Server Express: `Server=localhost\\SQLEXPRESS`
- For SQL Server LocalDB: `Server=(localdb)\\mssqllocaldb`
- For SQL Server: `Server=localhost` or your server name

### 2. Install EF Core Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

### 3. Create Initial Migration

Navigate to the Data project directory:

```bash
cd TheBeautyHubData
dotnet ef migrations add InitialCreate --startup-project ../TheBeautyHubAPI
```

### 4. Update Database

Apply the migration to create the database and tables:

```bash
dotnet ef database update --startup-project ../TheBeautyHubAPI
```

### 5. Deploy Stored Procedures

Execute the SQL scripts in [Scripts](TheBeautyHubData/Scripts/) folder on your database:

1. Open SQL Server Management Studio (SSMS) or Azure Data Studio
2. Connect to your SQL Server instance
3. Select the `TheBeautyHubDb` database
4. Execute the following scripts:
   - [AccountStoredProcedures.sql](TheBeautyHubData/Scripts/AccountStoredProcedures.sql)
   - [UserStoredProcedures.sql](TheBeautyHubData/Scripts/UserStoredProcedures.sql)

**Alternatively, use command line:**

```bash
sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i TheBeautyHubData\Scripts\AccountStoredProcedures.sql
sqlcmd -S localhost\SQLEXPRESS -d TheBeautyHubDb -i TheBeautyHubData\Scripts\UserStoredProcedures.sql
```

## Running the Application

### Start the API

```bash
cd TheBeautyHubAPI
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7xxx`
- HTTP: `http://localhost:5xxx`

Access Swagger UI at: `https://localhost:7xxx/swagger`

## API Endpoints

### Accounts

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/accounts` | Get all accounts |
| GET | `/api/accounts/{id}` | Get account by ID |
| GET | `/api/accounts/by-code/{code}` | Get account by code |
| POST | `/api/accounts` | Create new account |
| PUT | `/api/accounts/{id}` | Update account |
| DELETE | `/api/accounts/{id}` | Delete account (soft) |

### Users

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get user by ID |
| GET | `/api/users/by-email/{email}` | Get user by email |
| GET | `/api/users/by-account/{accountId}` | Get users by account |
| GET | `/api/users/by-manager/{managerId}` | Get users by manager |
| POST | `/api/users` | Create new user |
| PUT | `/api/users/{id}` | Update user |
| PUT | `/api/users/{id}/password` | Update user password |
| DELETE | `/api/users/{id}` | Delete user (soft) |

## Example API Calls

### Create Account

```bash
POST /api/accounts
Content-Type: application/json

{
  "accountCode": "ACC001",
  "accountName": "Beauty Salon Premium",
  "accountType": "FirmOwner",
  "mode": "subscription",
  "isUnderTrial": true,
  "trialStartedOn": "2026-01-17T00:00:00Z",
  "trialDuration": 30
}
```

### Create User

```bash
POST /api/users
Content-Type: application/json

{
  "accountId": "guid-from-account-creation",
  "userRole": "Admin",
  "userName": "John Doe",
  "userEmail": "john.doe@example.com",
  "userMobile": "+1234567890",
  "password": "SecurePassword123",
  "emailVerified": false,
  "mobileVerified": false,
  "status": "Active"
}
```

## Design Patterns Used

### Repository Pattern
- **Location**: `TheBeautyHubData/Repositories`
- **Purpose**: Abstracts data access logic
- **Implementation**: Each entity has an interface and implementation that uses stored procedures

### Service Pattern
- **Location**: `TheBeautyHubCore/Services`
- **Purpose**: Encapsulates business logic, validation, and orchestration
- **Implementation**: Services consume repository interfaces and implement business rules

### Dependency Injection
- **Location**: `TheBeautyHubAPI/Program.cs`
- **Purpose**: Loose coupling and testability
- **Registration**:
  - DbContext (Scoped)
  - Repositories (Scoped)
  - Services (Scoped)
  - AutoMapper (Singleton)

### DTO Pattern
- **Location**: `TheBeautyHubCore/DTOs` and `TheBeautyHubAPI/Models`
- **Purpose**: Separate internal data models from API contracts
- **Mapping**: AutoMapper for conversions

## Security Considerations

### Password Hashing
- Currently uses SHA256 (for demonstration)
- **RECOMMENDATION**: Replace with BCrypt or Argon2 for production:

```csharp
// Install BCrypt.Net-Next package
Install-Package BCrypt.Net-Next

// In UserService.cs
private byte[] HashPassword(string password)
{
    return Encoding.UTF8.GetBytes(BCrypt.Net.BCrypt.HashPassword(password));
}
```

### Validation
- Data annotations on request models
- Business validation in service layer
- Database constraints for data integrity

### Soft Deletes
- All deletes are soft deletes (IsDeleted flag)
- Data is never physically removed from database

## Testing with Swagger

1. Run the API: `dotnet run` in TheBeautyHubAPI folder
2. Open browser: `https://localhost:7xxx/swagger`
3. Try the endpoints:
   - First create an Account
   - Then create Users associated with that Account
   - Test GET, PUT, DELETE operations

## Adding New Tables

When you add new tables to the system, follow this pattern:

1. **Create Entity** in `TheBeautyHubData/Entities/`
2. **Add DbSet** to `BeautyHubDbContext`
3. **Configure Entity** in `OnModelCreating` method
4. **Create Stored Procedures** in `TheBeautyHubData/Scripts/`
5. **Create Repository Interface** in `TheBeautyHubData/Repositories/`
6. **Implement Repository** in `TheBeautyHubData/Repositories/`
7. **Create DTOs** in `TheBeautyHubCore/DTOs/`
8. **Create Service Interface** in `TheBeautyHubCore/Interfaces/`
9. **Implement Service** in `TheBeautyHubCore/Services/`
10. **Create Request/Response Models** in `TheBeautyHubAPI/Models/`
11. **Create Controller** in `TheBeautyHubAPI/Controllers/`
12. **Register DI** in `Program.cs`
13. **Add Mappings** to `MappingProfile.cs`
14. **Create Migration**: `dotnet ef migrations add AddXXXTable`
15. **Update Database**: `dotnet ef database update`

## Project Dependencies

### TheBeautyHubData
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.Design

### TheBeautyHubCore
- References: TheBeautyHubData

### TheBeautyHubAPI
- AutoMapper.Extensions.Microsoft.DependencyInjection
- References: TheBeautyHubData, TheBeautyHubCore

## Troubleshooting

### Migration Issues
```bash
# Remove last migration
dotnet ef migrations remove --startup-project ../TheBeautyHubAPI

# List migrations
dotnet ef migrations list --startup-project ../TheBeautyHubAPI
```

### Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure Windows Authentication is enabled (or use SQL auth)

### Stored Procedure Errors
- Ensure stored procedures are deployed to the database
- Check procedure names match exactly in repository code
- Verify parameter names and types

## Next Steps

1. **Add Authentication/Authorization**: Implement JWT tokens
2. **Add Logging**: Use Serilog or NLog
3. **Add Unit Tests**: Create test projects for each layer
4. **Add Pagination**: Implement paging for GET all endpoints
5. **Add Filtering/Sorting**: Enhance GET endpoints with query parameters
6. **Add Validation**: Implement FluentValidation
7. **Add Caching**: Implement response caching with Redis
8. **Add API Versioning**: Support multiple API versions

## Support

For issues or questions:
- Review the Swagger documentation
- Check the logs in the console output
- Verify database connectivity
- Ensure all migrations are applied
- Confirm stored procedures are deployed

---

**Version**: 1.0.0  
**Created**: January 2026  
**Database**: SQL Server Express Compatible
