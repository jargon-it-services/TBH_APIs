-- =============================================
-- Complete Database Setup Script
-- The Beauty Hub Application
-- =============================================
-- This script will:
-- 1. Drop and recreate the database
-- 2. Create all tables with constraints
-- 3. Deploy all stored procedures
-- =============================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'TheBeautyHub')
BEGIN
    ALTER DATABASE TheBeautyHub SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TheBeautyHub;
END
GO

-- Create database
CREATE DATABASE TheBeautyHub;
GO

USE TheBeautyHub;
GO

-- =============================================
-- Create Account Table
-- =============================================
CREATE TABLE Account (
    AccountId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    AccountCode VARCHAR(12) NOT NULL UNIQUE,
    AccountName NVARCHAR(200) NOT NULL,
    AccountType VARCHAR(20) NOT NULL CHECK (AccountType IN ('FirmOwner', 'Customer')),
    Mode VARCHAR(20) NOT NULL CHECK (Mode IN ('subscription', 'one_time')),
    IsUnderTrial BIT NOT NULL DEFAULT 0,
    TrialStartedOn DATETIME2(7) NULL,
    TrialDuration INT NULL,
    TrialExpiredOn DATETIME2(7) NULL,
    CreatedBy UNIQUEIDENTIFIER NULL,
    CreatedAt DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
    LastUpdated DATETIME2(7) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    
    CONSTRAINT CHK_AccountCode_Length CHECK (LEN(AccountCode) >= 6)
);
GO

-- =============================================
-- Create User Table
-- =============================================
CREATE TABLE [User] (
    UserId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    AccountId UNIQUEIDENTIFIER NOT NULL,
    UserRole VARCHAR(20) NOT NULL CHECK (UserRole IN ('Admin', 'Manager', 'Employee')),
    UserName NVARCHAR(150) NOT NULL,
    UserEmail NVARCHAR(256) NULL UNIQUE,
    UserMobile NVARCHAR(20) NULL UNIQUE,
    UserPasswordHash VARBINARY(64) NOT NULL,
    EmailVerified BIT NOT NULL DEFAULT 0,
    MobileVerified BIT NOT NULL DEFAULT 0,
    WorkerPaymentType VARCHAR(30) NULL CHECK (WorkerPaymentType IS NULL OR WorkerPaymentType IN ('Fix Pay', 'FP + Incentive', 'Incentive')),
    ManagerId UNIQUEIDENTIFIER NULL,
    CreatedBy UNIQUEIDENTIFIER NULL,
    CreatedAt DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
    LastUpdated DATETIME2(7) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    Status VARCHAR(20) NOT NULL DEFAULT 'Active',
    
    CONSTRAINT FK_User_Account FOREIGN KEY (AccountId) REFERENCES Account(AccountId),
    CONSTRAINT FK_User_Manager FOREIGN KEY (ManagerId) REFERENCES [User](UserId)
);
GO

-- Create indexes for better performance
CREATE INDEX IX_User_AccountId ON [User](AccountId);
CREATE INDEX IX_User_ManagerId ON [User](ManagerId);
CREATE INDEX IX_User_Email ON [User](UserEmail) WHERE UserEmail IS NOT NULL;
CREATE INDEX IX_User_Mobile ON [User](UserMobile) WHERE UserMobile IS NOT NULL;
GO

-- =============================================
-- Account Stored Procedures
-- =============================================

CREATE OR ALTER PROCEDURE usp_Insert_Account
    @AccountCode VARCHAR(12),
    @AccountName NVARCHAR(200),
    @AccountType VARCHAR(20),
    @Mode VARCHAR(20),
    @IsUnderTrial BIT = 0,
    @TrialStartedOn DATETIME2(7) = NULL,
    @TrialDuration INT = NULL,
    @TrialExpiredOn DATETIME2(7) = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewAccountId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO Account (
        AccountId, AccountCode, AccountName, AccountType, Mode,
        IsUnderTrial, TrialStartedOn, TrialDuration, TrialExpiredOn,
        CreatedBy, CreatedAt, IsDeleted
    )
    VALUES (
        @NewAccountId, @AccountCode, @AccountName, @AccountType, @Mode,
        @IsUnderTrial, @TrialStartedOn, @TrialDuration, @TrialExpiredOn,
        @CreatedBy, SYSUTCDATETIME(), 0
    );
    
    SELECT * FROM Account WHERE AccountId = @NewAccountId;
END
GO

CREATE OR ALTER PROCEDURE usp_Update_Account
    @AccountId UNIQUEIDENTIFIER,
    @AccountCode VARCHAR(12),
    @AccountName NVARCHAR(200),
    @AccountType VARCHAR(20),
    @Mode VARCHAR(20),
    @IsUnderTrial BIT,
    @TrialStartedOn DATETIME2(7) = NULL,
    @TrialDuration INT = NULL,
    @TrialExpiredOn DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Account
    SET AccountCode = @AccountCode, AccountName = @AccountName,
        AccountType = @AccountType, Mode = @Mode,
        IsUnderTrial = @IsUnderTrial, TrialStartedOn = @TrialStartedOn,
        TrialDuration = @TrialDuration, TrialExpiredOn = @TrialExpiredOn,
        LastUpdated = SYSUTCDATETIME()
    WHERE AccountId = @AccountId AND IsDeleted = 0;
    
    SELECT * FROM Account WHERE AccountId = @AccountId;
END
GO

CREATE OR ALTER PROCEDURE usp_Delete_Account
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Account
    SET IsDeleted = 1, LastUpdated = SYSUTCDATETIME()
    WHERE AccountId = @AccountId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_AccountById
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Account WHERE AccountId = @AccountId AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_AllAccounts
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Account WHERE IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_AccountByCode
    @AccountCode VARCHAR(12)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Account WHERE AccountCode = @AccountCode AND IsDeleted = 0;
END
GO

-- =============================================
-- User Stored Procedures
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
        UserId, AccountId, UserRole, UserName, UserEmail, UserMobile,
        UserPasswordHash, EmailVerified, MobileVerified, WorkerPaymentType,
        ManagerId, CreatedBy, CreatedAt, IsDeleted, Status
    )
    VALUES (
        @NewUserId, @AccountId, @UserRole, @UserName, @UserEmail, @UserMobile,
        @UserPasswordHash, @EmailVerified, @MobileVerified, @WorkerPaymentType,
        @ManagerId, @CreatedBy, SYSUTCDATETIME(), 0, @Status
    );
    
    SELECT * FROM [User] WHERE UserId = @NewUserId;
END
GO

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
    SET UserRole = @UserRole, UserName = @UserName,
        UserEmail = @UserEmail, UserMobile = @UserMobile,
        EmailVerified = @EmailVerified, MobileVerified = @MobileVerified,
        WorkerPaymentType = @WorkerPaymentType, ManagerId = @ManagerId,
        Status = @Status, LastUpdated = SYSUTCDATETIME()
    WHERE UserId = @UserId AND IsDeleted = 0;
    
    SELECT * FROM [User] WHERE UserId = @UserId;
END
GO

CREATE OR ALTER PROCEDURE usp_Update_UserPassword
    @UserId UNIQUEIDENTIFIER,
    @UserPasswordHash VARBINARY(64)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [User]
    SET UserPasswordHash = @UserPasswordHash, LastUpdated = SYSUTCDATETIME()
    WHERE UserId = @UserId AND IsDeleted = 0;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

CREATE OR ALTER PROCEDURE usp_Delete_User
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [User]
    SET IsDeleted = 1, LastUpdated = SYSUTCDATETIME()
    WHERE UserId = @UserId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_UserById
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [User] WHERE UserId = @UserId AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_AllUsers
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [User] WHERE IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_UsersByAccountId
    @AccountId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [User] WHERE AccountId = @AccountId AND IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_UserByEmail
    @UserEmail NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [User] WHERE UserEmail = @UserEmail AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE usp_Get_UsersByManagerId
    @ManagerId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [User] WHERE ManagerId = @ManagerId AND IsDeleted = 0 ORDER BY CreatedAt DESC;
END
GO

PRINT 'Database setup completed successfully!';
PRINT 'Database: TheBeautyHubDb';
PRINT 'Tables created: Account, User';
PRINT 'Stored procedures deployed: 15 procedures';
GO
