using System;

namespace TheBeautyHubCore.DTOs
{
    /// <summary>
    /// DTO for User data transfer
    /// </summary>
    public class UserDto
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

    /// <summary>
    /// DTO for creating a new user
    /// </summary>
    public class CreateUserDto
    {
        public Guid AccountId { get; set; }
        public string UserRole { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string? UserMobile { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public string? WorkerPaymentType { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO for updating an existing user
    /// </summary>
    public class UpdateUserDto
    {
        public Guid UserId { get; set; }
        public string UserRole { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string? UserMobile { get; set; }
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public string? WorkerPaymentType { get; set; }
        public Guid? ManagerId { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO for updating user password
    /// </summary>
    public class UpdateUserPasswordDto
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
