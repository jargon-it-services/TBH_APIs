-- =============================================
-- Stored Procedures for User Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_User
-- Description: Inserts a new user record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_User
    @AccountId UNIQUEIDENTIFIER,
    @UserRole VARCHAR(20),
    @UserName NVARCHAR(150),
    @UserEmail NVARCHAR(256) = NULL,
    @UserMobile NVARCHAR(20) = NULL,
    @UserPasswordHash VARBINARY(64),
    @EmailVerified BIT = 0,
    @MobileVerified BIT = 0,
    @WorkerPaymentType VARCHAR(30) = NULL,
    @ManagerId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @Status VARCHAR(20) = 'Active'
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewUserId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO [User] (
        UserId,
        AccountId,
        UserRole,
        UserName,
        UserEmail,
        UserMobile,
        UserPasswordHash,
        EmailVerified,
        MobileVerified,
        WorkerPaymentType,
        ManagerId,
        CreatedBy,
        CreatedAt,
        IsDeleted,
        Status
    )
    VALUES (
        @NewUserId,
        @AccountId,
        @UserRole,
        @UserName,
        @UserEmail,
        @UserMobile,
        @UserPasswordHash,
        @EmailVerified,
        @MobileVerified,
        @WorkerPaymentType,
        @ManagerId,
        @CreatedBy,
        SYSUTCDATETIME(),
        0,
        @Status
    );
    
    -- Return the newly created user
    SELECT * FROM [User] WHERE UserId = @NewUserId;
END
GO

-- =============================================
-- Procedure: usp_Update_User
-- Description: Updates an existing user record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_User
    @UserId UNIQUEIDENTIFIER,
    @UserRole VARCHAR(20),
    @UserName NVARCHAR(150),
    @UserEmail NVARCHAR(256) = NULL,
    @UserMobile NVARCHAR(20) = NULL,
    @EmailVerified BIT,
    @MobileVerified BIT,
    @WorkerPaymentType VARCHAR(30) = NULL,
    @ManagerId UNIQUEIDENTIFIER = NULL,
    @Status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [User]
    SET
        UserRole = @UserRole,
        UserName = @UserName,
        UserEmail = @UserEmail,
        UserMobile = @UserMobile,
        EmailVerified = @EmailVerified,
        MobileVerified = @MobileVerified,
        WorkerPaymentType = @WorkerPaymentType,
        ManagerId = @ManagerId,
        Status = @Status,
        LastUpdated = SYSUTCDATETIME()
    WHERE UserId = @UserId AND IsDeleted = 0;
    
    -- Return the updated user
    SELECT * FROM [User] WHERE UserId = @UserId;
END
GO

-- =============================================
-- Procedure: usp_Update_UserPassword
-- Description: Updates user password hash
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_UserPassword
    @UserId UNIQUEIDENTIFIER,
    @UserPasswordHash VARBINARY(64)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [User]
    SET
        UserPasswordHash = @UserPasswordHash,
        LastUpdated = SYSUTCDATETIME()
    WHERE UserId = @UserId AND IsDeleted = 0;
    
    -- Return affected rows count
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Delete_User
-- Description: Soft deletes a user (sets IsDeleted = 1)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_User
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [User]
    SET
        IsDeleted = 1,
        LastUpdated = SYSUTCDATETIME()
    WHERE UserId = @UserId;
    
    -- Return affected rows count
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_UserById
-- Description: Retrieves a user by their ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_UserById
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM [User]
    WHERE UserId = @UserId AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_AllUsers
-- Description: Retrieves all non-deleted users
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_AllUsers
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM [User]
    WHERE IsDeleted = 0
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_UsersByAccountId
-- Description: Retrieves all users for a specific account
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_UsersByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM [User]
    WHERE AccountId = @AccountId AND IsDeleted = 0
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_UserByEmail
-- Description: Retrieves a user by their email
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_UserByEmail
    @UserEmail NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM [User]
    WHERE UserEmail = @UserEmail AND IsDeleted = 0;
END
GO

-- =============================================
-- Procedure: usp_Get_UsersByManagerId
-- Description: Retrieves all users managed by a specific manager
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_UsersByManagerId
    @ManagerId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM [User]
    WHERE ManagerId = @ManagerId AND IsDeleted = 0
    ORDER BY CreatedAt DESC;
END
GO
