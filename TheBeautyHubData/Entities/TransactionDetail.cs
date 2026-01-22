using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a detail line item for a transaction.
    /// Contains information about services, expenses, and incentives.
    /// </summary>
    [Table("TransactionsDetails")]
    public class TransactionDetail
    {
        /// <summary>
        /// Unique identifier for the transaction detail
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionDetailsId { get; set; }

        /// <summary>
        /// Foreign key to the parent Transaction
        /// </summary>
        [Required]
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Foreign key to TransactionType
        /// </summary>
        [Required]
        public Guid TransactionTypeId { get; set; }

        /// <summary>
        /// Foreign key to ExpensesType (optional)
        /// </summary>
        public Guid? ExpensesTypeId { get; set; }

        /// <summary>
        /// Foreign key to Service (optional)
        /// </summary>
        public Guid? ServiceId { get; set; }

        /// <summary>
        /// Amount for this detail line
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Incentive amount (optional)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? IncentiveAmount { get; set; }

        /// <summary>
        /// Foreign key to TransactionRules (optional)
        /// </summary>
        public Guid? TransactionRuleId { get; set; }

        /// <summary>
        /// Foreign key to the Account
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Foreign key to the Firm (optional)
        /// </summary>
        public Guid? FirmId { get; set; }

        /// <summary>
        /// User ID who created this detail
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when detail was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when detail was last updated
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        /// <summary>
        /// The parent transaction
        /// </summary>
        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; } = null!;

        /// <summary>
        /// The transaction type
        /// </summary>
        [ForeignKey("TransactionTypeId")]
        public virtual TransactionType TransactionType { get; set; } = null!;

        /// <summary>
        /// The expenses type (optional)
        /// </summary>
        [ForeignKey("ExpensesTypeId")]
        public virtual ExpensesType? ExpensesType { get; set; }

        /// <summary>
        /// The service (optional)
        /// </summary>
        [ForeignKey("ServiceId")]
        public virtual Services? Service { get; set; }

        /// <summary>
        /// The transaction rule applied (optional)
        /// </summary>
        [ForeignKey("TransactionRuleId")]
        public virtual TransactionRules? TransactionRule { get; set; }

        /// <summary>
        /// The account (optional)
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }

        /// <summary>
        /// The firm (optional)
        /// </summary>
        [ForeignKey("FirmId")]
        public virtual Firm? Firm { get; set; }
    }
}
