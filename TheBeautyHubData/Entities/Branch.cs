using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a salon/spa branch location.
    /// </summary>
    [Table("Branch")]
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BranchId { get; set; }

        public Guid? AccountId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string AddressLine1 { get; set; } = string.Empty;

        [StringLength(500)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Pincode { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string BranchType { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string OpeningTime { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string ClosingTime { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string WeeklyOff { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active";

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Longitude { get; set; }

        [StringLength(500)]
        public string? MapsLink { get; set; }

        [StringLength(500)]
        public string? Logo { get; set; }

        public Guid? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdated { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }

        public virtual ICollection<BranchService> BranchServices { get; set; } = new List<BranchService>();

        public virtual ICollection<BranchEmployee> BranchEmployees { get; set; } = new List<BranchEmployee>();
    }
}
