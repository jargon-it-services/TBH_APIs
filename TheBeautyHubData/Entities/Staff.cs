using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBeautyHubData.Enums;

namespace TheBeautyHubData.Entities
{
    [Table("Staff")]
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StaffId { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        public Guid BranchId { get; set; }

        [Required]
        public Guid SalaryRuleId { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AadhaarNumber { get; set; } = string.Empty;

        [StringLength(30)]
        public string? EmployeeCode { get; set; }

        public DateTime? JoiningDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Specialist { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = RecordStatus.Active.ToApiValue();

        [Required]
        public bool AllowAppLogin { get; set; }

        [StringLength(20)]
        public string? AppRole { get; set; }

        [StringLength(100)]
        public string? Username { get; set; }

        [StringLength(500)]
        public string? Photo { get; set; }

        [StringLength(500)]
        public string? AadhaarCardUrl { get; set; }

        public Guid? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdated { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;

        [ForeignKey("SalaryRuleId")]
        public virtual SalaryRule SalaryRule { get; set; } = null!;
    }
}
