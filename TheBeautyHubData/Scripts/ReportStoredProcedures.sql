-- =============================================
-- Stored Procedures for Reports Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Report
-- Description: Inserts a new report record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Report
    @ReportName NVARCHAR(200),
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewReportId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Reports (
        ReportId, ReportName, IsActive, CreatedAt
    )
    VALUES (
        @NewReportId, @ReportName, @IsActive, SYSUTCDATETIME()
    );
    
    SELECT * FROM Reports WHERE ReportId = @NewReportId;
END
GO

-- =============================================
-- Procedure: usp_Update_Report
-- Description: Updates an existing report record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Report
    @ReportId UNIQUEIDENTIFIER,
    @ReportName NVARCHAR(200),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Reports
    SET ReportName = @ReportName,
        IsActive = @IsActive,
        LastUpdated = SYSUTCDATETIME()
    WHERE ReportId = @ReportId;
    
    SELECT * FROM Reports WHERE ReportId = @ReportId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Report
-- Description: Deletes a report (hard delete since no IsDeleted column)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Report
    @ReportId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Reports WHERE ReportId = @ReportId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_ReportById
-- Description: Retrieves a report by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ReportById
    @ReportId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Reports WHERE ReportId = @ReportId;
END
GO

-- =============================================
-- Procedure: usp_Get_AllReports
-- Description: Retrieves all reports
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllReports
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Reports ORDER BY ReportName;
END
GO

-- =============================================
-- Procedure: usp_Get_ActiveReports
-- Description: Retrieves all active reports
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ActiveReports
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Reports WHERE IsActive = 1 ORDER BY ReportName;
END
GO
