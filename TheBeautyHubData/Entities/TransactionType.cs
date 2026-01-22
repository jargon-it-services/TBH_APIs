using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a Transaction Type in the system.
    /// Categorizes transactions as Service or Expenses.
    /// </summary>
    [Table("TransactionType")]
    public class TransactionType
    {
        /// <summary>
        /// Unique identifier for the transaction type
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionTypeId { get; set; }

        /// <summary>
        /// Type of transaction: Service or Expenses
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when transaction type was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates if this transaction type is active
        /// </summary>
        [Required]
        public bool IsTransactionTypeActive { get; set; } = true;

        /// <summary>
        /// Date and time when transaction type was last updated
        /// </summary>
        [Column(TypeName = "datetime2(7)")]
        public DateTime? LastUpdated { get; set; }

        // Navigation property
        /// <summary>
        /// Collection of services using this transaction type
        /// </summary>
        public virtual ICollection<Services> Services { get; set; } = new List<Services>();
    }
}
