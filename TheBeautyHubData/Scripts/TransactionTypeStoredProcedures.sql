-- =============================================
-- Stored Procedures for TransactionType Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_TransactionType
-- Description: Inserts a new transaction type record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_TransactionType
    @TransactionType VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewTransactionTypeId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO TransactionType (
        TransactionTypeId, Type, CreatedAt, IsTransactionTypeActive
    )
    VALUES (
        @NewTransactionTypeId, @TransactionType, SYSUTCDATETIME(), 1
    );
    
    SELECT * FROM TransactionType WHERE TransactionTypeId = @NewTransactionTypeId;
END
GO

-- =============================================
-- Procedure: usp_Update_TransactionType
-- Description: Updates an existing transaction type record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_TransactionType
    @TransactionTypeId UNIQUEIDENTIFIER,
    @TransactionType VARCHAR(20),
    @IsTransactionTypeActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TransactionType
    SET Type = @TransactionType,
        IsTransactionTypeActive = @IsTransactionTypeActive,
        LastUpdated = SYSUTCDATETIME()
    WHERE TransactionTypeId = @TransactionTypeId;
    
    SELECT * FROM TransactionType WHERE TransactionTypeId = @TransactionTypeId;
END
GO

-- =============================================
-- Procedure: usp_Delete_TransactionType
-- Description: Deletes a transaction type
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_TransactionType
    @TransactionTypeId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM TransactionType WHERE TransactionTypeId = @TransactionTypeId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionTypeById
-- Description: Retrieves a transaction type by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionTypeById
    @TransactionTypeId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionType WHERE TransactionTypeId = @TransactionTypeId;
END
GO

-- =============================================
-- Procedure: usp_Get_AllTransactionTypes
-- Description: Retrieves all transaction types
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllTransactionTypes
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionType ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_ActiveTransactionTypes
-- Description: Retrieves all active transaction types
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ActiveTransactionTypes
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionType WHERE IsTransactionTypeActive = 1 ORDER BY CreatedAt DESC;
END
GO

