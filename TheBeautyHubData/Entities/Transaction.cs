using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBeautyHubData.Enums;

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
        public string Status { get; set; } = TransactionStatus.Pending.ToApiValue();

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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when transaction was last updated
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Date when transaction was posted
        /// </summary>
        public DateTime? PostedDate { get; set; }

        /// <summary>
        /// Date of the transaction
        /// </summary>
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Check-in time
        /// </summary>
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// Check-out time
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        [StringLength(20)]
        public string? Code { get; set; }

        [StringLength(20)]
        public string? Type { get; set; }

        public Guid? BranchId { get; set; }

        [StringLength(30)]
        public string? PaymentMode { get; set; }

        [StringLength(150)]
        public string? CustomerName { get; set; }

        [StringLength(20)]
        public string? CustomerMobile { get; set; }

        [StringLength(1000)]
        public string? Remark { get; set; }

        public Guid? StaffId { get; set; }

        [StringLength(50)]
        public string? CouponCode { get; set; }

        [StringLength(20)]
        public string? CouponType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CouponValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CouponDiscount { get; set; }

        [StringLength(80)]
        public string? IdempotencyKey { get; set; }

        public int EditCount { get; set; }

        public DateTime? EditableUntil { get; set; }

        [StringLength(150)]
        public string? LastEditedBy { get; set; }

        public DateTime? LastEditedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxPercentage { get; set; }

        // Navigation properties
        /// <summary>
        /// The account associated with this transaction
        /// </summary>

        /// <summary>
        /// The firm associated with this transaction (optional)
        /// </summary>
        [ForeignKey("FirmId")]
        public virtual Firm? Firm { get; set; }

        /// <summary>
        /// Collection of transaction details
        /// </summary>
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

        [ForeignKey("BranchId")]
        public virtual Branch? Branch { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff? Staff { get; set; }
    }
}
