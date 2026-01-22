-- =============================================
-- Stored Procedures for TransactionRules Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_TransactionRules
-- Description: Inserts a new transaction rule record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_TransactionRules
    @RuleName NVARCHAR(200),
    @AccountId UNIQUEIDENTIFIER = NULL,
    @FirmId UNIQUEIDENTIFIER = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewTransactionRuleId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO TransactionRules (
        TransactionRuleId, RuleName, AccountId, FirmId, CreatedAt, IsActive
    )
    VALUES (
        @NewTransactionRuleId, @RuleName, @AccountId, @FirmId, SYSUTCDATETIME(), @IsActive
    );
    
    SELECT * FROM TransactionRules WHERE TransactionRuleId = @NewTransactionRuleId;
END
GO

-- =============================================
-- Procedure: usp_Update_TransactionRules
-- Description: Updates an existing transaction rule record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_TransactionRules
    @TransactionRuleId UNIQUEIDENTIFIER,
    @RuleName NVARCHAR(200),
    @AccountId UNIQUEIDENTIFIER = NULL,
    @FirmId UNIQUEIDENTIFIER = NULL,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TransactionRules
    SET RuleName = @RuleName,
        AccountId = @AccountId,
        FirmId = @FirmId,
        IsActive = @IsActive
    WHERE TransactionRuleId = @TransactionRuleId;
    
    SELECT * FROM TransactionRules WHERE TransactionRuleId = @TransactionRuleId;
END
GO

-- =============================================
-- Procedure: usp_Delete_TransactionRules
-- Description: Deletes a transaction rule
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_TransactionRules
    @TransactionRuleId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM TransactionRules WHERE TransactionRuleId = @TransactionRuleId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionRulesById
-- Description: Retrieves a transaction rule by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionRulesById
    @TransactionRuleId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionRules WHERE TransactionRuleId = @TransactionRuleId;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionRulesByAccountId
-- Description: Retrieves all transaction rules for an account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionRulesByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionRules WHERE AccountId = @AccountId ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_AllTransactionRules
-- Description: Retrieves all transaction rules
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllTransactionRules
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionRules ORDER BY CreatedAt DESC;
END
GO
