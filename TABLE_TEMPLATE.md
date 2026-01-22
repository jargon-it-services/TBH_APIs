# New Table Template

When you're ready to add your next table, provide the information in this format:

## Table Name: [YourTableName]

### Columns

| Column Name | Data Type | Constraints / Notes |
|-------------|-----------|---------------------|
| [Id] | UNIQUEIDENTIFIER | PK, DEFAULT NEWID() |
| [Column1] | VARCHAR(50) | NOT NULL |
| [Column2] | NVARCHAR(200) | NULL |
| [Column3] | INT | NOT NULL, DEFAULT 0 |
| [ForeignKeyId] | UNIQUEIDENTIFIER | NOT NULL, FK -> OtherTable(Id) |
| CreatedBy | UNIQUEIDENTIFIER | NULL |
| CreatedAt | DATETIME2(7) | NOT NULL DEFAULT SYSUTCDATETIME() |
| LastUpdated | DATETIME2(7) | NULL |
| IsDeleted | BIT | NOT NULL DEFAULT 0 |

### Relationships
- **Table1** → **Table2**: One-to-Many (via ForeignKeyId)
- **Table1** → **Table3**: Many-to-One

### Check Constraints
- Status must be in ('Active', 'Inactive', 'Pending')
- Amount must be >= 0

### Unique Constraints
- Column1 + Column2 combination must be unique

### Notes
- [Any special business rules or requirements]

---

## Example: Service Table

### Columns

| Column Name | Data Type | Constraints / Notes |
|-------------|-----------|---------------------|
| ServiceId | UNIQUEIDENTIFIER | PK, DEFAULT NEWID() |
| AccountId | UNIQUEIDENTIFIER | NOT NULL, FK -> Account(AccountId) |
| ServiceName | NVARCHAR(200) | NOT NULL |
| ServiceCode | VARCHAR(20) | UNIQUE, NOT NULL |
| Description | NVARCHAR(500) | NULL |
| Duration | INT | NOT NULL (in minutes) |
| Price | DECIMAL(10,2) | NOT NULL |
| IsActive | BIT | NOT NULL DEFAULT 1 |
| CreatedBy | UNIQUEIDENTIFIER | NULL |
| CreatedAt | DATETIME2(7) | NOT NULL DEFAULT SYSUTCDATETIME() |
| LastUpdated | DATETIME2(7) | NULL |
| IsDeleted | BIT | NOT NULL DEFAULT 0 |

### Relationships
- **Service** → **Account**: Many-to-One (one account can have many services)

### Check Constraints
- Duration > 0
- Price >= 0

### Unique Constraints
- ServiceCode must be unique

### Notes
- Services represent beauty treatments offered by salons
- Duration is in minutes
- Price is in local currency

---

## What I'll Generate for Your Table

When you provide a table definition, I will create:

### 1. Data Layer (TheBeautyHubData)
- ✅ Entity class with proper attributes
- ✅ DbSet in BeautyHubDbContext
- ✅ Entity configuration (relationships, constraints)
- ✅ Repository interface (IYourTableRepository)
- ✅ Repository implementation (YourTableRepository)
- ✅ SQL stored procedures script

### 2. Business Layer (TheBeautyHubCore)
- ✅ DTOs (YourTableDto, CreateYourTableDto, UpdateYourTableDto)
- ✅ Service interface (IYourTableService)
- ✅ Service implementation with validation (YourTableService)

### 3. API Layer (TheBeautyHubAPI)
- ✅ Request models (CreateYourTableRequest, UpdateYourTableRequest)
- ✅ Response model (YourTableResponse)
- ✅ Controller (YourTablesController)
- ✅ AutoMapper mappings

### 4. Database Updates
- ✅ EF Core migration command
- ✅ Database update command

---

## Ready to Add Your Next Table?

Simply provide:
1. Table name
2. Column definitions (as shown in the table above)
3. Any special relationships
4. Business rules or constraints

I'll generate all the necessary code following the same pattern used for Account and User tables!

---

**Tip**: You can provide multiple tables at once, and I'll add them all systematically.
