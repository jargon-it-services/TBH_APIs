namespace TheBeautyHubAPI.Auth
{
    public class AuthCenterOptions
    {
        public const string SectionName = "AuthCenter";

        /// <summary>
        /// AuthCenter API base URL, e.g. http://localhost:5070
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        public string ValidatePath { get; set; } = "/api/auth/validate";

        public string ProfilePath { get; set; } = "/api/user/profile";

        /// <summary>
        /// Application name registered in AuthCenter for TBH logins.
        /// </summary>
        public string ApplicationName { get; set; } = AuthCenterClaimTypes.BeautyHubApplicationName;

        /// <summary>
        /// When true, each request also confirms the session with AuthCenter (revoked tokens fail).
        /// </summary>
        public bool ValidateWithAuthCenter { get; set; } = true;

        public int ValidateCacheSeconds { get; set; } = 30;
    }
}
