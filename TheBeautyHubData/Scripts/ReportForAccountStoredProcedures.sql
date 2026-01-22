-- =============================================
-- Stored Procedures for ReportsForAccount Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_ReportForAccount
-- Description: Inserts a new report-for-account record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_ReportForAccount
    @ReportId UNIQUEIDENTIFIER,
    @AccountId UNIQUEIDENTIFIER,
    @IsActive BIT = 1,
    @CreatedBy UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO ReportsForAccount (
        Id, ReportId, AccountId, IsActive, CreatedAt, CreatedBy
    )
    VALUES (
        @NewId, @ReportId, @AccountId, @IsActive, SYSUTCDATETIME(), @CreatedBy
    );
    
    SELECT * FROM ReportsForAccount WHERE Id = @NewId;
END
GO

-- =============================================
-- Procedure: usp_Update_ReportForAccount
-- Description: Updates an existing report-for-account record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_ReportForAccount
    @Id UNIQUEIDENTIFIER,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE ReportsForAccount
    SET IsActive = @IsActive,
        LastUpdated = SYSUTCDATETIME()
    WHERE Id = @Id;
    
    SELECT * FROM ReportsForAccount WHERE Id = @Id;
END
GO

-- =============================================
-- Procedure: usp_Delete_ReportForAccount
-- Description: Deletes a report-for-account record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_ReportForAccount
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM ReportsForAccount WHERE Id = @Id;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_ReportForAccountById
-- Description: Retrieves a report-for-account by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ReportForAccountById
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ReportsForAccount WHERE Id = @Id;
END
GO

-- =============================================
-- Procedure: usp_Get_ReportsByAccountId
-- Description: Retrieves all reports for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ReportsByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT rfa.*, r.ReportName 
    FROM ReportsForAccount rfa
    INNER JOIN Reports r ON rfa.ReportId = r.ReportId
    WHERE rfa.AccountId = @AccountId 
    ORDER BY r.ReportName;
END
GO
