-- =============================================
-- Stored Procedures for ExpensesType Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_ExpensesType
-- Description: Inserts a new expense type record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_ExpensesType
    @AccountId UNIQUEIDENTIFIER,
    @ExpensesTypeName NVARCHAR(200),
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @FirmId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewExpensesTypeId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO ExpensesType (
        ExpensesTypeId, AccountId, ExpensesTypeName, CreatedBy, CreatedAt, IsDeleted, FirmId
    )
    VALUES (
        @NewExpensesTypeId, @AccountId, @ExpensesTypeName, @CreatedBy, SYSUTCDATETIME(), 0, @FirmId
    );
    
    SELECT * FROM ExpensesType WHERE ExpensesTypeId = @NewExpensesTypeId;
END
GO

-- =============================================
-- Procedure: usp_Update_ExpensesType
-- Description: Updates an existing expense type record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_ExpensesType
    @ExpensesTypeId UNIQUEIDENTIFIER,
    @ExpensesTypeName NVARCHAR(200),
    @FirmId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE ExpensesType
    SET ExpensesTypeName = @ExpensesTypeName,
        FirmId = @FirmId,
        LastUpdated = SYSUTCDATETIME()
    WHERE ExpensesTypeId = @ExpensesTypeId AND IsDeleted = 0;
    
    SELECT * FROM ExpensesType WHERE ExpensesTypeId = @ExpensesTypeId;
END
GO

-- =============================================
-- Procedure: usp_Delete_ExpensesType
-- Description: Soft deletes an expense type
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_ExpensesType
    @ExpensesTypeId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE ExpensesType
    SET IsDeleted = 1, LastUpdated = SYSUTCDATETIME()
    WHERE ExpensesTypeId = @ExpensesTypeId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_ExpensesTypeById
-- Description: Retrieves an expense type by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ExpensesTypeById
    @ExpensesTypeId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ExpensesType WHERE ExpensesTypeId = @ExpensesTypeId AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_ExpensesTypesByAccountId
-- Description: Retrieves all expense types for an account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ExpensesTypesByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ExpensesType WHERE AccountId = @AccountId AND IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_AllExpensesTypes
-- Description: Retrieves all non-deleted expense types
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllExpensesTypes
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ExpensesType WHERE IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO
