-- =============================================
-- Stored Procedures for Account Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Account
-- Description: Inserts a new account record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Account
    @AccountCode VARCHAR(12),
    @AccountName NVARCHAR(200),
    @AccountType VARCHAR(20),
    @Mode VARCHAR(20),
    @IsUnderTrial BIT = 0,
    @TrialStartedOn DATETIME2(7) = NULL,
    @TrialDuration INT = NULL,
    @TrialExpiredOn DATETIME2(7) = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewAccountId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Account (
        AccountId,
        AccountCode,
        AccountName,
        AccountType,
        Mode,
        IsUnderTrial,
        TrialStartedOn,
        TrialDuration,
        TrialExpiredOn,
        CreatedBy,
        CreatedAt,
        IsDeleted
    )
    VALUES (
        @NewAccountId,
        @AccountCode,
        @AccountName,
        @AccountType,
        @Mode,
        @IsUnderTrial,
        @TrialStartedOn,
        @TrialDuration,
        @TrialExpiredOn,
        @CreatedBy,
        SYSUTCDATETIME(),
        0
    );
    
    -- Return the newly created account
    SELECT * FROM Account WHERE AccountId = @NewAccountId;
END
GO

-- =============================================
-- Procedure: usp_Update_Account
-- Description: Updates an existing account record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Account
    @AccountId UNIQUEIDENTIFIER,
    @AccountCode VARCHAR(12),
    @AccountName NVARCHAR(200),
    @AccountType VARCHAR(20),
    @Mode VARCHAR(20),
    @IsUnderTrial BIT,
    @TrialStartedOn DATETIME2(7) = NULL,
    @TrialDuration INT = NULL,
    @TrialExpiredOn DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Account
    SET
        AccountCode = @AccountCode,
        AccountName = @AccountName,
        AccountType = @AccountType,
        Mode = @Mode,
        IsUnderTrial = @IsUnderTrial,
        TrialStartedOn = @TrialStartedOn,
        TrialDuration = @TrialDuration,
        TrialExpiredOn = @TrialExpiredOn,
        LastUpdated = SYSUTCDATETIME()
    WHERE AccountId = @AccountId AND IsDeleted = 0;
    
    -- Return the updated account
    SELECT * FROM Account WHERE AccountId = @AccountId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Account
-- Description: Soft deletes an account (sets IsDeleted = 1)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Account
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Account
    SET
        IsDeleted = 1,
        LastUpdated = SYSUTCDATETIME()
    WHERE AccountId = @AccountId;
    
    -- Return affected rows count
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_AccountById
-- Description: Retrieves an account by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AccountById
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Account
    WHERE AccountId = @AccountId AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_AllAccounts
-- Description: Retrieves all non-deleted accounts
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllAccounts
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Account
    WHERE IsDeleted = 0
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_AccountByCode
-- Description: Retrieves an account by its unique code
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AccountByCode
    @AccountCode VARCHAR(12)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Account
    WHERE AccountCode = @AccountCode AND IsDeleted = 0;
END
GO
