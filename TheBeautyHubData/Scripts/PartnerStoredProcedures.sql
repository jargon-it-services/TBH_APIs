-- =============================================
-- Stored Procedures for Partner Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Partner
-- Description: Inserts a new partner record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Partner
    @Name NVARCHAR(150),
    @Type NVARCHAR(50) = NULL,
    @Address NVARCHAR(500) = NULL,
    @Mobile NVARCHAR(20) = NULL,
    @Email NVARCHAR(256) = NULL,
    @DateofBirth DATE = NULL,
    @Gender VARCHAR(20) = NULL,
    @AccountId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewPartnerId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Partner (
        PartnerId, Name, Type, Address, Mobile, Email,
        DateofBirth, Gender, AccountId, CreatedAt
    )
    VALUES (
        @NewPartnerId, @Name, @Type, @Address, @Mobile, @Email,
        @DateofBirth, @Gender, @AccountId, SYSUTCDATETIME()
    );
    
    SELECT * FROM Partner WHERE PartnerId = @NewPartnerId;
END
GO

-- =============================================
-- Procedure: usp_Update_Partner
-- Description: Updates an existing partner record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Partner
    @PartnerId UNIQUEIDENTIFIER,
    @Name NVARCHAR(150),
    @Type NVARCHAR(50) = NULL,
    @Address NVARCHAR(500) = NULL,
    @Mobile NVARCHAR(20) = NULL,
    @Email NVARCHAR(256) = NULL,
    @DateofBirth DATE = NULL,
    @Gender VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Partner
    SET Name = @Name,
        Type = @Type,
        Address = @Address,
        Mobile = @Mobile,
        Email = @Email,
        DateofBirth = @DateofBirth,
        Gender = @Gender
    WHERE PartnerId = @PartnerId;
    
    SELECT * FROM Partner WHERE PartnerId = @PartnerId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Partner
-- Description: Deletes a partner (hard delete since no IsDeleted column)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Partner
    @PartnerId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Partner WHERE PartnerId = @PartnerId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_PartnerById
-- Description: Retrieves a partner by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_PartnerById
    @PartnerId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Partner WHERE PartnerId = @PartnerId;
END
GO

-- =============================================
-- Procedure: usp_Get_AllPartners
-- Description: Retrieves all partners
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllPartners
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Partner ORDER BY Name;
END
GO

-- =============================================
-- Procedure: usp_Get_PartnersByAccountId
-- Description: Retrieves all partners for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_PartnersByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Partner 
    WHERE AccountId = @AccountId 
    ORDER BY Name;
END
GO

-- =============================================
-- Procedure: usp_Get_PartnerByEmail
-- Description: Retrieves a partner by email
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_PartnerByEmail
    @Email NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Partner WHERE Email = @Email;
END
GO
