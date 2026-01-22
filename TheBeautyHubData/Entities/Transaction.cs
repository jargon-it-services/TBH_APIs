using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a transaction in the system.
    /// Tracks financial transactions with status and timestamps.
    /// </summary>
    [Table("Transactions")]
    public class Transaction
    {
        /// <summary>
        /// Unique identifier for the transaction
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Transaction status: Draft, Posted, or Cancelled
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Total amount of the transaction
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        /// <summary>
        /// Foreign key to the Account
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Foreign key to the Firm (optional)
        /// </summary>
        public Guid? FirmId { get; set; }

        /// <summary>
        /// User ID who created this transaction
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when transaction was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when transaction was last updated
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Date when transaction was posted
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? PostedDate { get; set; }

        /// <summary>
        /// Date of the transaction
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Check-in time
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// Check-out time
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? CheckOutTime { get; set; }

        // Navigation properties
        /// <summary>
        /// The account associated with this transaction
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;

        /// <summary>
        /// The firm associated with this transaction (optional)
        /// </summary>
        [ForeignKey("FirmId")]
        public virtual Firm? Firm { get; set; }

        /// <summary>
        /// Collection of transaction details
        /// </summary>
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }
}
