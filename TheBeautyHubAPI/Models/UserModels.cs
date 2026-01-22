using System;
using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    /// <summary>
    /// Request model for creating a new user
    /// </summary>
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Account ID is required")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "User role is required")]
        [RegularExpression("^(Admin|Manager|Employee)$", ErrorMessage = "User role must be 'Admin', 'Manager', or 'Employee'")]
        public string UserRole { get; set; } = string.Empty;

        [Required(ErrorMessage = "User name is required")]
        [StringLength(150, ErrorMessage = "User name cannot exceed 150 characters")]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
        public string? UserEmail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters")]
        public string? UserMobile { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = string.Empty;

        public bool EmailVerified { get; set; }
        
        public bool MobileVerified { get; set; }

        [RegularExpression("^(Fix Pay|FP \\+ Incentive|Incentive)$", ErrorMessage = "Worker payment type must be 'Fix Pay', 'FP + Incentive', or 'Incentive'")]
        public string? WorkerPaymentType { get; set; }

        public Guid? ManagerId { get; set; }
        
        public Guid? CreatedBy { get; set; }

        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// Request model for updating an existing user
    /// </summary>
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "User role is required")]
        [RegularExpression("^(Admin|Manager|Employee)$", ErrorMessage = "User role must be 'Admin', 'Manager', or 'Employee'")]
        public string UserRole { get; set; } = string.Empty;

        [Required(ErrorMessage = "User name is required")]
        [StringLength(150, ErrorMessage = "User name cannot exceed 150 characters")]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
        public string? UserEmail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters")]
        public string? UserMobile { get; set; }

        public bool EmailVerified { get; set; }
        
        public bool MobileVerified { get; set; }

        [RegularExpression("^(Fix Pay|FP \\+ Incentive|Incentive)$", ErrorMessage = "Worker payment type must be 'Fix Pay', 'FP + Incentive', or 'Incentive'")]
        public string? WorkerPaymentType { get; set; }

        public Guid? ManagerId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// Request model for updating user password
    /// </summary>
    public class UpdateUserPasswordRequest
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for user data
    /// </summary>
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public string UserRole { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string? UserMobile { get; set; }
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public string? WorkerPaymentType { get; set; }
        public Guid? ManagerId { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
