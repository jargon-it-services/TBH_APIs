-- =============================================
-- Stored Procedures for Services Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Services
-- Description: Inserts a new service record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Services
    @ServiceName NVARCHAR(200),
    @ServiceDescription NVARCHAR(1000) = NULL,
    @ServicePrice DECIMAL(18,2) = 0,
    @ServiceTypeId UNIQUEIDENTIFIER = NULL,
    @AccountId UNIQUEIDENTIFIER,
    @FirmId UNIQUEIDENTIFIER = NULL,
    @IsIncentiveApplicable BIT = 0,
    @IncentiveAmount DECIMAL(18,2) = NULL,
    @IncentivePercentage DECIMAL(5,2) = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewServiceId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Services (
        ServiceId, ServiceName, ServiceDescription, ServicePrice, ServiceTypeId,
        AccountId, FirmId, IsIncentiveApplicable, IncentiveAmount, IncentivePercentage,
        CreatedBy, CreatedAt, IsDeleted
    )
    VALUES (
        @NewServiceId, @ServiceName, @ServiceDescription, @ServicePrice, @ServiceTypeId,
        @AccountId, @FirmId, @IsIncentiveApplicable, @IncentiveAmount, @IncentivePercentage,
        @CreatedBy, SYSUTCDATETIME(), 0
    );
    
    SELECT * FROM Services WHERE ServiceId = @NewServiceId;
END
GO

-- =============================================
-- Procedure: usp_Update_Services
-- Description: Updates an existing service record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Services
    @ServiceId UNIQUEIDENTIFIER,
    @ServiceName NVARCHAR(200),
    @ServiceDescription NVARCHAR(1000) = NULL,
    @ServicePrice DECIMAL(18,2),
    @ServiceTypeId UNIQUEIDENTIFIER = NULL,
    @FirmId UNIQUEIDENTIFIER = NULL,
    @IsIncentiveApplicable BIT,
    @IncentiveAmount DECIMAL(18,2) = NULL,
    @IncentivePercentage DECIMAL(5,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Services
    SET ServiceName = @ServiceName,
        ServiceDescription = @ServiceDescription,
        ServicePrice = @ServicePrice,
        ServiceTypeId = @ServiceTypeId,
        FirmId = @FirmId,
        IsIncentiveApplicable = @IsIncentiveApplicable,
        IncentiveAmount = @IncentiveAmount,
        IncentivePercentage = @IncentivePercentage,
        LastUpdated = SYSUTCDATETIME()
    WHERE ServiceId = @ServiceId AND IsDeleted = 0;
    
    SELECT * FROM Services WHERE ServiceId = @ServiceId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Services
-- Description: Soft deletes a service
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Services
    @ServiceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Services
    SET IsDeleted = 1, LastUpdated = SYSUTCDATETIME()
    WHERE ServiceId = @ServiceId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_ServicesById
-- Description: Retrieves a service by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ServicesById
    @ServiceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Services WHERE ServiceId = @ServiceId AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_ServicesByAccountId
-- Description: Retrieves all services for an account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ServicesByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Services WHERE AccountId = @AccountId AND IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_AllServices
-- Description: Retrieves all non-deleted services
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllServices
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Services WHERE IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO
