-- =============================================
-- Stored Procedures for ExceptionLogs Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_ExceptionLog
-- Description: Inserts a new exception log record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_ExceptionLog
    @Type NVARCHAR(100),
    @ErrorMessage NVARCHAR(MAX),
    @DeviceName NVARCHAR(100) = NULL,
    @UserId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO ExceptionLogs (
        Type, ErrorMessage, DeviceName, UserId, CreatedAt
    )
    VALUES (
        @Type, @ErrorMessage, @DeviceName, @UserId, SYSUTCDATETIME()
    );
    
    -- Return the newly inserted ID
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

-- =============================================
-- Procedure: usp_Delete_ExceptionLog
-- Description: Deletes an exception log record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_ExceptionLog
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM ExceptionLogs WHERE Id = @Id;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_ExceptionLogById
-- Description: Retrieves an exception log by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ExceptionLogById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ExceptionLogs WHERE Id = @Id;
END
GO

-- =============================================
-- Procedure: usp_Get_AllExceptionLogs
-- Description: Retrieves all exception logs (with pagination recommended)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllExceptionLogs
    @PageSize INT = 100,
    @PageNumber INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ExceptionLogs 
    ORDER BY CreatedAt DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- =============================================
-- Procedure: usp_Get_ExceptionLogsByUserId
-- Description: Retrieves all exception logs for a specific user
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ExceptionLogsByUserId
    @UserId UNIQUEIDENTIFIER,
    @PageSize INT = 100,
    @PageNumber INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ExceptionLogs 
    WHERE UserId = @UserId
    ORDER BY CreatedAt DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- =============================================
-- Procedure: usp_Get_ExceptionLogsByType
-- Description: Retrieves all exception logs of a specific type
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ExceptionLogsByType
    @Type NVARCHAR(100),
    @PageSize INT = 100,
    @PageNumber INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM ExceptionLogs 
    WHERE Type = @Type
    ORDER BY CreatedAt DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- =============================================
-- Procedure: usp_Delete_OldExceptionLogs
-- Description: Deletes exception logs older than specified days
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_OldExceptionLogs
    @DaysToKeep INT = 90
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM ExceptionLogs 
    WHERE CreatedAt < DATEADD(DAY, -@DaysToKeep, SYSUTCDATETIME());
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO
