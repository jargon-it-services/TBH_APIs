using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents Transaction Rules in the system.
    /// Defines business rules for transaction processing at account or firm level.
    /// </summary>
    [Table("TransactionRules")]
    public class TransactionRules
    {
        /// <summary>
        /// Unique identifier for the transaction rule
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionRuleId { get; set; }

        /// <summary>
        /// Name of the rule
        /// </summary>
        [Required]
        [StringLength(200)]
        public string RuleName { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to the Account (optional)
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Foreign key to the Firm (optional)
        /// </summary>
        public Guid? FirmId { get; set; }

        /// <summary>
        /// Date and time when rule was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if the rule is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        /// <summary>
        /// The account this rule applies to (optional)
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }

        /// <summary>
        /// The firm this rule applies to (optional)
        /// </summary>
        [ForeignKey("FirmId")]
        public virtual Firm? Firm { get; set; }
    }
}
