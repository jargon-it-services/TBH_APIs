-- =============================================
-- Stored Procedures for Firm Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Firm
-- Description: Inserts a new firm record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Firm
    @AccountId UNIQUEIDENTIFIER,
    @FirmName NVARCHAR(200),
    @FirmAddress NVARCHAR(500) = NULL,
    @FirmGstin VARCHAR(15) = NULL,
    @FirmContact NVARCHAR(20) = NULL,
    @FirmEmail NVARCHAR(256) = NULL,
    @FirmPhoto NVARCHAR(500) = NULL,
    @FirmOwnerName NVARCHAR(150) = NULL,
    @FirmType NVARCHAR(50) = NULL,
    @FirmRegistration NVARCHAR(100) = NULL,
    @FirmLogo NVARCHAR(500) = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewFirmId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Firm (
        FirmId,
        AccountId,
        FirmName,
        FirmAddress,
        FirmGstin,
        FirmContact,
        FirmEmail,
        FirmPhoto,
        FirmOwnerName,
        FirmType,
        FirmRegistration,
        FirmLogo,
        CreatedBy,
        CreatedAt,
        IsDeleted
    )
    VALUES (
        @NewFirmId,
        @AccountId,
        @FirmName,
        @FirmAddress,
        @FirmGstin,
        @FirmContact,
        @FirmEmail,
        @FirmPhoto,
        @FirmOwnerName,
        @FirmType,
        @FirmRegistration,
        @FirmLogo,
        @CreatedBy,
        SYSUTCDATETIME(),
        0
    );
    
    -- Return the newly created firm
    SELECT * FROM Firm WHERE FirmId = @NewFirmId;
END
GO

-- =============================================
-- Procedure: usp_Update_Firm
-- Description: Updates an existing firm record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Firm
    @FirmId UNIQUEIDENTIFIER,
    @FirmName NVARCHAR(200),
    @FirmAddress NVARCHAR(500) = NULL,
    @FirmGstin VARCHAR(15) = NULL,
    @FirmContact NVARCHAR(20) = NULL,
    @FirmEmail NVARCHAR(256) = NULL,
    @FirmPhoto NVARCHAR(500) = NULL,
    @FirmOwnerName NVARCHAR(150) = NULL,
    @FirmType NVARCHAR(50) = NULL,
    @FirmRegistration NVARCHAR(100) = NULL,
    @FirmLogo NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Firm
    SET
        FirmName = @FirmName,
        FirmAddress = @FirmAddress,
        FirmGstin = @FirmGstin,
        FirmContact = @FirmContact,
        FirmEmail = @FirmEmail,
        FirmPhoto = @FirmPhoto,
        FirmOwnerName = @FirmOwnerName,
        FirmType = @FirmType,
        FirmRegistration = @FirmRegistration,
        FirmLogo = @FirmLogo,
        LastUpdated = SYSUTCDATETIME()
    WHERE FirmId = @FirmId AND IsDeleted = 0;
    
    -- Return the updated firm
    SELECT * FROM Firm WHERE FirmId = @FirmId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Firm
-- Description: Soft deletes a firm (sets IsDeleted = 1)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Firm
    @FirmId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Firm
    SET
        IsDeleted = 1,
        LastUpdated = SYSUTCDATETIME()
    WHERE FirmId = @FirmId;
    
    -- Return affected rows count
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_FirmById
-- Description: Retrieves a firm by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_FirmById
    @FirmId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Firm
    WHERE FirmId = @FirmId AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_AllFirms
-- Description: Retrieves all non-deleted firms
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllFirms
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Firm
    WHERE IsDeleted = 0
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_FirmsByAccountId
-- Description: Retrieves all firms for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_FirmsByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Firm
    WHERE AccountId = @AccountId AND IsDeleted = 0
    ORDER BY CreatedAt DESC;
END
GO
