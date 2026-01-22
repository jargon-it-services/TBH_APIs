using System;
using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    /// <summary>
    /// Request model for creating a new account
    /// </summary>
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = "Account code is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Account code must be between 6 and 12 characters")]
        public string AccountCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(200, ErrorMessage = "Account name cannot exceed 200 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type is required")]
        [RegularExpression("^(FirmOwner|Customer)$", ErrorMessage = "Account type must be 'FirmOwner' or 'Customer'")]
        public string AccountType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mode is required")]
        [RegularExpression("^(subscription|one_time)$", ErrorMessage = "Mode must be 'subscription' or 'one_time'")]
        public string Mode { get; set; } = string.Empty;

        public bool IsUnderTrial { get; set; }
        
        public DateTime? TrialStartedOn { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Trial duration must be greater than 0")]
        public int? TrialDuration { get; set; }
        
        public DateTime? TrialExpiredOn { get; set; }
        
        public Guid? CreatedBy { get; set; }
    }

    /// <summary>
    /// Request model for updating an existing account
    /// </summary>
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = "Account ID is required")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Account code is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Account code must be between 6 and 12 characters")]
        public string AccountCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(200, ErrorMessage = "Account name cannot exceed 200 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type is required")]
        [RegularExpression("^(FirmOwner|Customer)$", ErrorMessage = "Account type must be 'FirmOwner' or 'Customer'")]
        public string AccountType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mode is required")]
        [RegularExpression("^(subscription|one_time)$", ErrorMessage = "Mode must be 'subscription' or 'one_time'")]
        public string Mode { get; set; } = string.Empty;

        public bool IsUnderTrial { get; set; }
        
        public DateTime? TrialStartedOn { get; set; }
        
        public int? TrialDuration { get; set; }
        
        public DateTime? TrialExpiredOn { get; set; }
    }

    /// <summary>
    /// Response model for account data
    /// </summary>
    public class AccountResponse
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
}
