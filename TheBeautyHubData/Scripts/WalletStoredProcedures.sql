-- =============================================
-- Stored Procedures for Wallet Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Wallet
-- Description: Inserts a new wallet record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Wallet
    @AccountId UNIQUEIDENTIFIER,
    @Amount DECIMAL(18,2) = 0,
    @WalletType VARCHAR(30),
    @IsUsed BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewWalletId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Wallet (
        WalletId, AccountId, Amount, WalletType, IsUsed, CreatedAt
    )
    VALUES (
        @NewWalletId, @AccountId, @Amount, @WalletType, @IsUsed, SYSUTCDATETIME()
    );
    
    SELECT * FROM Wallet WHERE WalletId = @NewWalletId;
END
GO

-- =============================================
-- Procedure: usp_Update_Wallet
-- Description: Updates an existing wallet record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Wallet
    @WalletId UNIQUEIDENTIFIER,
    @Amount DECIMAL(18,2),
    @WalletType VARCHAR(30),
    @IsUsed BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Wallet
    SET Amount = @Amount,
        WalletType = @WalletType,
        IsUsed = @IsUsed
    WHERE WalletId = @WalletId;
    
    SELECT * FROM Wallet WHERE WalletId = @WalletId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Wallet
-- Description: Deletes a wallet record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Wallet
    @WalletId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Wallet WHERE WalletId = @WalletId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_WalletById
-- Description: Retrieves a wallet by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_WalletById
    @WalletId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Wallet WHERE WalletId = @WalletId;
END
GO

-- =============================================
-- Procedure: usp_Get_WalletsByAccountId
-- Description: Retrieves all wallets for an account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_WalletsByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Wallet WHERE AccountId = @AccountId ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_AllWallets
-- Description: Retrieves all wallets
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllWallets
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Wallet ORDER BY CreatedAt DESC;
END
GO
