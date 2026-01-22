-- =============================================
-- Stored Procedures for FirmDetails Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_FirmDetails
-- Description: Inserts a new firm details record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_FirmDetails
    @UserId UNIQUEIDENTIFIER,
    @AccountId UNIQUEIDENTIFIER,
    @FirmId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewFirmDetailsId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO FirmDetails (
        FirmDetailsId,
        UserId,
        AccountId,
        FirmId,
        CreatedAt
    )
    VALUES (
        @NewFirmDetailsId,
        @UserId,
        @AccountId,
        @FirmId,
        SYSUTCDATETIME()
    );
    
    -- Return the newly created firm details
    SELECT * FROM FirmDetails WHERE FirmDetailsId = @NewFirmDetailsId;
END
GO

-- =============================================
-- Procedure: usp_Update_FirmDetails
-- Description: Updates an existing firm details record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_FirmDetails
    @FirmDetailsId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @AccountId UNIQUEIDENTIFIER,
    @FirmId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE FirmDetails
    SET
        UserId = @UserId,
        AccountId = @AccountId,
        FirmId = @FirmId
    WHERE FirmDetailsId = @FirmDetailsId;
    
    -- Return the updated firm details
    SELECT * FROM FirmDetails WHERE FirmDetailsId = @FirmDetailsId;
END
GO

-- =============================================
-- Procedure: usp_Delete_FirmDetails
-- Description: Deletes a firm details record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_FirmDetails
    @FirmDetailsId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM FirmDetails
    WHERE FirmDetailsId = @FirmDetailsId;
    
    -- Return affected rows count
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_FirmDetailsById
-- Description: Retrieves firm details by ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_FirmDetailsById
    @FirmDetailsId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM FirmDetails
    WHERE FirmDetailsId = @FirmDetailsId;
END
GO

-- =============================================
-- Procedure: usp_Get_AllFirmDetails
-- Description: Retrieves all firm details records
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllFirmDetails
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM FirmDetails
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_FirmDetailsByFirmId
-- Description: Retrieves all firm details for a specific firm
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_FirmDetailsByFirmId
    @FirmId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM FirmDetails
    WHERE FirmId = @FirmId
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_FirmDetailsByUserId
-- Description: Retrieves all firm details for a specific user
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_FirmDetailsByUserId
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM FirmDetails
    WHERE UserId = @UserId
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_FirmDetailsByAccountId
-- Description: Retrieves all firm details for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_FirmDetailsByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM FirmDetails
    WHERE AccountId = @AccountId
    ORDER BY CreatedAt DESC;
END
GO
