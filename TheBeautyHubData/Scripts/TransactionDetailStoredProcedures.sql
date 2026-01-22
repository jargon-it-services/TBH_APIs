-- =============================================
-- Stored Procedures for TransactionsDetails Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_TransactionDetail
-- Description: Inserts a new transaction detail record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_TransactionDetail
    @TransactionId UNIQUEIDENTIFIER,
    @TransactionTypeId UNIQUEIDENTIFIER,
    @ExpensesTypeId UNIQUEIDENTIFIER = NULL,
    @ServiceId UNIQUEIDENTIFIER = NULL,
    @Amount DECIMAL(18,2),
    @IncentiveAmount DECIMAL(18,2) = NULL,
    @TransactionRuleId UNIQUEIDENTIFIER = NULL,
    @AccountId UNIQUEIDENTIFIER = NULL,
    @FirmId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewDetailId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO TransactionsDetails (
        TransactionDetailsId, TransactionId, TransactionTypeId, ExpensesTypeId,
        ServiceId, Amount, IncentiveAmount, TransactionRuleId, AccountId,
        FirmId, CreatedBy, CreatedAt, IsDeleted
    )
    VALUES (
        @NewDetailId, @TransactionId, @TransactionTypeId, @ExpensesTypeId,
        @ServiceId, @Amount, @IncentiveAmount, @TransactionRuleId, @AccountId,
        @FirmId, @CreatedBy, SYSUTCDATETIME(), 0
    );
    
    SELECT * FROM TransactionsDetails WHERE TransactionDetailsId = @NewDetailId;
END
GO

-- =============================================
-- Procedure: usp_Update_TransactionDetail
-- Description: Updates an existing transaction detail record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_TransactionDetail
    @TransactionDetailsId UNIQUEIDENTIFIER,
    @TransactionTypeId UNIQUEIDENTIFIER,
    @ExpensesTypeId UNIQUEIDENTIFIER = NULL,
    @ServiceId UNIQUEIDENTIFIER = NULL,
    @Amount DECIMAL(18,2),
    @IncentiveAmount DECIMAL(18,2) = NULL,
    @TransactionRuleId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TransactionsDetails
    SET TransactionTypeId = @TransactionTypeId,
        ExpensesTypeId = @ExpensesTypeId,
        ServiceId = @ServiceId,
        Amount = @Amount,
        IncentiveAmount = @IncentiveAmount,
        TransactionRuleId = @TransactionRuleId,
        LastUpdated = SYSUTCDATETIME()
    WHERE TransactionDetailsId = @TransactionDetailsId AND IsDeleted = 0;
    
    SELECT * FROM TransactionsDetails WHERE TransactionDetailsId = @TransactionDetailsId;
END
GO

-- =============================================
-- Procedure: usp_Delete_TransactionDetail
-- Description: Soft deletes a transaction detail (sets IsDeleted = 1)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_TransactionDetail
    @TransactionDetailsId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TransactionsDetails
    SET IsDeleted = 1,
        LastUpdated = SYSUTCDATETIME()
    WHERE TransactionDetailsId = @TransactionDetailsId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionDetailById
-- Description: Retrieves a transaction detail by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionDetailById
    @TransactionDetailsId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionsDetails 
    WHERE TransactionDetailsId = @TransactionDetailsId AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionDetailsByTransactionId
-- Description: Retrieves all details for a specific transaction
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionDetailsByTransactionId
    @TransactionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM TransactionsDetails 
    WHERE TransactionId = @TransactionId AND IsDeleted = 0 
    ORDER BY CreatedAt;
END
GO
