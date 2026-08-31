using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBeautyHubData.Enums;

namespace TheBeautyHubData.Entities
{
    [Table("SalaryRule")]
    public class SalaryRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SalaryRuleId { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SalaryType { get; set; } = Enums.SalaryType.Fixed.ToStoredDefault();

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FixedSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MonthlyTarget { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TargetBonus { get; set; }

        [Required]
        public bool AllowAdvanceRecovery { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxRecoveryPerMonth { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = RecordStatus.Active.ToApiValue();

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdated { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

    }
}
