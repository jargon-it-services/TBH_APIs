-- =============================================
-- Stored Procedures for UserSessions Table
-- =============================================

-- =============================================
-- Procedure: usp_Insert_UserSession
-- Description: Inserts a new user session record
-- =============================================
CREATE OR ALTER PROCEDURE usp_Insert_UserSession
    @UserId UNIQUEIDENTIFIER,
    @IpAddress NVARCHAR(45) = NULL,
    @UserAgent NVARCHAR(256) = NULL,
    @DeviceId NVARCHAR(128) = NULL,
    @AccessTokenJti UNIQUEIDENTIFIER,
    @RefreshTokenHash VARBINARY(32),
    @RefreshTokenExpiresAt DATETIME2(7)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewSessionId UNIQUEIDENTIFIER = NEWID();
    
    INSERT INTO UserSessions (
        SessionId, UserId, CreatedAt, LastSeenAt, IpAddress,
        UserAgent, DeviceId, AccessTokenJti, RefreshTokenHash,
        RefreshTokenExpiresAt
    )
    VALUES (
        @NewSessionId, @UserId, SYSUTCDATETIME(), SYSUTCDATETIME(), @IpAddress,
        @UserAgent, @DeviceId, @AccessTokenJti, @RefreshTokenHash,
        @RefreshTokenExpiresAt
    );
    
    SELECT * FROM UserSessions WHERE SessionId = @NewSessionId;
END
GO

-- =============================================
-- Procedure: usp_Update_UserSession
-- Description: Updates a user session (mainly for LastSeenAt)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_UserSession
    @SessionId UNIQUEIDENTIFIER,
    @LastSeenAt DATETIME2(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE UserSessions
    SET LastSeenAt = ISNULL(@LastSeenAt, SYSUTCDATETIME())
    WHERE SessionId = @SessionId;
    
    SELECT * FROM UserSessions WHERE SessionId = @SessionId;
END
GO

-- =============================================
-- Procedure: usp_Revoke_UserSession
-- Description: Revokes a user session
-- =============================================
CREATE OR ALTER PROCEDURE usp_Revoke_UserSession
    @SessionId UNIQUEIDENTIFIER,
    @RevocationReason NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE UserSessions
    SET RevokedAt = SYSUTCDATETIME(),
        RevocationReason = @RevocationReason
    WHERE SessionId = @SessionId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Delete_UserSession
-- Description: Deletes a user session (hard delete)
-- =============================================
CREATE OR ALTER PROCEDURE usp_Delete_UserSession
    @SessionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM UserSessions WHERE SessionId = @SessionId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- =============================================
-- Procedure: usp_Get_UserSessionById
-- Description: Retrieves a user session by its ID
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_UserSessionById
    @SessionId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM UserSessions WHERE SessionId = @SessionId;
END
GO

-- =============================================
-- Procedure: usp_Get_UserSessionsByUserId
-- Description: Retrieves all sessions for a specific user
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_UserSessionsByUserId
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM UserSessions 
    WHERE UserId = @UserId 
    ORDER BY CreatedAt DESC;
END
GO

-- =============================================
-- Procedure: usp_Get_ActiveUserSessions
-- Description: Retrieves all active (non-revoked) sessions for a user
-- =============================================
CREATE OR ALTER PROCEDURE usp_Get_ActiveUserSessions
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM UserSessions 
    WHERE UserId = @UserId 
        AND RevokedAt IS NULL 
        AND RefreshTokenExpiresAt > SYSUTCDATETIME()
    ORDER BY LastSeenAt DESC;
END
GO
