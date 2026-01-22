-- =============================================
-- Stored Procedures for Transactions Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Transaction
-- Description: Inserts a new transaction record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Transaction
    @Status VARCHAR(20) = 'Draft',
    @TotalAmount DECIMAL(18,2) = 0,
    @AccountId UNIQUEIDENTIFIER,
    @FirmId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @PostedDate DATETIME2(7) = NULL,
    @TransactionDate DATETIME2(7) = NULL,
    @CheckInTime DATETIME2(7) = NULL,
    @CheckOutTime DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewTransactionId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Transactions (
        TransactionId, Status, TotalAmount, AccountId, FirmId,
        CreatedBy, CreatedAt, IsDeleted, PostedDate, TransactionDate,
        CheckInTime, CheckOutTime
    )
    VALUES (
        @NewTransactionId, @Status, @TotalAmount, @AccountId, @FirmId,
        @CreatedBy, SYSUTCDATETIME(), 0, @PostedDate, @TransactionDate,
        @CheckInTime, @CheckOutTime
    );
    
    SELECT * FROM Transactions WHERE TransactionId = @NewTransactionId;
END
GO

-- =============================================
-- Procedure: usp_Update_Transaction
-- Description: Updates an existing transaction record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Transaction
    @TransactionId UNIQUEIDENTIFIER,
    @Status VARCHAR(20),
    @TotalAmount DECIMAL(18,2),
    @PostedDate DATETIME2(7) = NULL,
    @TransactionDate DATETIME2(7) = NULL,
    @CheckInTime DATETIME2(7) = NULL,
    @CheckOutTime DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Transactions
    SET Status = @Status,
        TotalAmount = @TotalAmount,
        PostedDate = @PostedDate,
        TransactionDate = @TransactionDate,
        CheckInTime = @CheckInTime,
        CheckOutTime = @CheckOutTime,
        LastUpdated = SYSUTCDATETIME()
    WHERE TransactionId = @TransactionId AND IsDeleted = 0;
    
    SELECT * FROM Transactions WHERE TransactionId = @TransactionId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Transaction
-- Description: Soft deletes a transaction (sets IsDeleted = 1)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Transaction
    @TransactionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Transactions
    SET IsDeleted = 1,
        LastUpdated = SYSUTCDATETIME()
    WHERE TransactionId = @TransactionId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionById
-- Description: Retrieves a transaction by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionById
    @TransactionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Transactions WHERE TransactionId = @TransactionId AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_AllTransactions
-- Description: Retrieves all non-deleted transactions
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllTransactions
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Transactions WHERE IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionsByAccountId
-- Description: Retrieves all transactions for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionsByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Transactions 
    WHERE AccountId = @AccountId AND IsDeleted = 0 
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_TransactionsByFirmId
-- Description: Retrieves all transactions for a specific firm
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_TransactionsByFirmId
    @FirmId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Transactions 
    WHERE FirmId = @FirmId AND IsDeleted = 0 
    ORDER BY CreatedAt DESC;
END
GO
