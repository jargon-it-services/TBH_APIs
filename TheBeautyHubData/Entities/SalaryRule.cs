using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdated { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;
    }
}
