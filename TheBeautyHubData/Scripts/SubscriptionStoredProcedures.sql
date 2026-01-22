-- =============================================
-- Stored Procedures for Subscription Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_Subscription
-- Description: Inserts a new subscription record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_Subscription
    @AccountId UNIQUEIDENTIFIER,
    @PlanId UNIQUEIDENTIFIER,
    @Status VARCHAR(20) = 'Pending',
    @ExpiryOn DATETIME2(7) = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @SubscriptionAmount DECIMAL(18,2),
    @DiscountedAmount DECIMAL(18,2) = 0,
    @SubscriptionAmountAfterDiscount DECIMAL(18,2),
    @DiscountType VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewSubscriptionId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Subscription (
        SubscriptionId,
        AccountId,
        PlanId,
        Status,
        ExpiryOn,
        CreatedBy,
        CreatedAt,
        SubscriptionAmount,
        DiscountedAmount,
        SubscriptionAmountAfterDiscount,
        DiscountType
    )
    VALUES (
        @NewSubscriptionId,
        @AccountId,
        @PlanId,
        @Status,
        @ExpiryOn,
        @CreatedBy,
        SYSUTCDATETIME(),
        @SubscriptionAmount,
        @DiscountedAmount,
        @SubscriptionAmountAfterDiscount,
        @DiscountType
    );
    
    -- Return the newly created subscription
    SELECT * FROM Subscription WHERE SubscriptionId = @NewSubscriptionId;
END
GO

-- =============================================
-- Procedure: usp_Update_Subscription
-- Description: Updates an existing subscription record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Subscription
    @SubscriptionId UNIQUEIDENTIFIER,
    @Status VARCHAR(20),
    @ExpiryOn DATETIME2(7) = NULL,
    @SubscriptionAmount DECIMAL(18,2),
    @DiscountedAmount DECIMAL(18,2),
    @SubscriptionAmountAfterDiscount DECIMAL(18,2),
    @DiscountType VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Subscription
    SET
        Status = @Status,
        ExpiryOn = @ExpiryOn,
        SubscriptionAmount = @SubscriptionAmount,
        DiscountedAmount = @DiscountedAmount,
        SubscriptionAmountAfterDiscount = @SubscriptionAmountAfterDiscount,
        DiscountType = @DiscountType
    WHERE SubscriptionId = @SubscriptionId;
    
    -- Return the updated subscription
    SELECT * FROM Subscription WHERE SubscriptionId = @SubscriptionId;
END
GO

-- =============================================
-- Procedure: usp_Delete_Subscription
-- Description: Deletes a subscription (sets status to Cancelled)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_Subscription
    @SubscriptionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Subscription
    SET Status = 'Cancelled'
    WHERE SubscriptionId = @SubscriptionId;
    
    -- Return affected rows count
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_SubscriptionById
-- Description: Retrieves a subscription by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_SubscriptionById
    @SubscriptionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Subscription
    WHERE SubscriptionId = @SubscriptionId;
END
GO

-- =============================================
-- Procedure: usp_Get_AllSubscriptions
-- Description: Retrieves all subscriptions
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllSubscriptions
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Subscription
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_SubscriptionsByAccountId
-- Description: Retrieves all subscriptions for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_SubscriptionsByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Subscription
    WHERE AccountId = @AccountId
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_ActiveSubscriptionsByAccountId
-- Description: Retrieves active subscriptions for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ActiveSubscriptionsByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Subscription
    WHERE AccountId = @AccountId 
      AND Status = 'Active'
      AND (ExpiryOn IS NULL OR ExpiryOn > SYSUTCDATETIME())
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_SubscriptionsByPlanId
-- Description: Retrieves all subscriptions for a specific plan
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_SubscriptionsByPlanId
    @PlanId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Subscription
    WHERE PlanId = @PlanId
    ORDER BY CreatedAt DESC;
END
GO

