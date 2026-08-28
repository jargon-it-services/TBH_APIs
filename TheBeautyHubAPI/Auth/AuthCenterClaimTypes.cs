namespace TheBeautyHubAPI.Auth
{
    /// <summary>
    /// Claim types issued by AuthCenter JWTs. Must stay aligned with AuthCenter AppConstants.
    /// </summary>
    public static class AuthCenterClaimTypes
    {
        public const string UserId = "userId";
        public const string AccountId = "accountId";
        public const string Email = "email";
        public const string Name = "name";
        public const string Role = "role";
        public const string SessionId = "sessionId";
        public const string ApplicationId = "applicationId";
        public const string Permission = "permission";
        public const string SigningKeyId = "AuthCenter";
        public const string BeautyHubApplicationName = "The Beauty Hub";
        public const string RawTokenItemKey = "AuthCenter.RawAccessToken";
    }
}
