using System;

namespace TheBeautyHubCore.DTOs
{
    /// <summary>
    /// DTO for Account creation and updates
    /// </summary>
    public class AccountDto
    {
        public Guid AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public bool IsUnderTrial { get; set; }
        public DateTime? TrialStartedOn { get; set; }
        public int? TrialDuration { get; set; }
        public DateTime? TrialExpiredOn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// DTO for creating a new account
    /// </summary>
    public class CreateAccountDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public bool IsUnderTrial { get; set; }
        public DateTime? TrialStartedOn { get; set; }
        public int? TrialDuration { get; set; }
        public DateTime? TrialExpiredOn { get; set; }
        public Guid? CreatedBy { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing account
    /// </summary>
    public class UpdateAccountDto
    {
        public Guid AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public bool IsUnderTrial { get; set; }
        public DateTime? TrialStartedOn { get; set; }
        public int? TrialDuration { get; set; }
        public DateTime? TrialExpiredOn { get; set; }
    }
}
