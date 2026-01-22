-- =============================================
-- Stored Procedures for Plans Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Plan
-- Description: Inserts a new plan record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Plan
    @PlanName NVARCHAR(200),
    @PlanDescription NVARCHAR(1000) = NULL,
    @PlanCost DECIMAL(18,2),
    @IsPlanActive BIT = 1,
    @PlanAppliedTo NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewPlanId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Plans (
        PlanId,
        PlanName,
        PlanDescription,
        PlanCost,
        CreatedAt,
        IsPlanActive,
        PlanAppliedTo
    )
    VALUES (
        @NewPlanId,
        @PlanName,
        @PlanDescription,
        @PlanCost,
        SYSUTCDATETIME(),
        @IsPlanActive,
        @PlanAppliedTo
    );
    
    -- Return the newly created plan
    SELECT * FROM Plans WHERE PlanId = @NewPlanId;
END
GO

-- =============================================
-- Procedure: usp_Update_Plan
-- Description: Updates an existing plan record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Plan
    @PlanId UNIQUEIDENTIFIER,
    @PlanName NVARCHAR(200),
    @PlanDescription NVARCHAR(1000) = NULL,
    @PlanCost DECIMAL(18,2),
    @IsPlanActive BIT,
    @PlanAppliedTo NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Plans
    SET
        PlanName = @PlanName,
        PlanDescription = @PlanDescription,
        PlanCost = @PlanCost,
        IsPlanActive = @IsPlanActive,
        PlanAppliedTo = @PlanAppliedTo,
        LastUpdated = SYSUTCDATETIME()
    WHERE PlanId = @PlanId;
    
    -- Return the updated plan
    SELECT * FROM Plans WHERE PlanId = @PlanId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Plan
-- Description: Deletes a plan (hard delete - use with caution)
-- Note: Consider adding IsDeleted flag for soft delete instead
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Plan
    @PlanId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Soft delete approach: deactivate the plan instead of deleting
    UPDATE Plans
    SET IsPlanActive = 0, LastUpdated = SYSUTCDATETIME()
    WHERE PlanId = @PlanId;
    
    -- Return affected rows count
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_PlanById
-- Description: Retrieves a plan by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_PlanById
    @PlanId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Plans
    WHERE PlanId = @PlanId;
END
GO

-- =============================================
-- Procedure: usp_Get_AllPlans
-- Description: Retrieves all plans
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllPlans
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Plans
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_ActivePlans
-- Description: Retrieves all active plans
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ActivePlans
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Plans
    WHERE IsPlanActive = 1
    ORDER BY PlanCost ASC;
END
GO
