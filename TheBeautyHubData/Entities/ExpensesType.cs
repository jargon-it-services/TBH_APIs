using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents an Expense Type in the system.
    /// Defines categories of expenses for accounting and tracking purposes.
    /// </summary>
    [Table("ExpensesType")]
    public class ExpensesType
    {
        /// <summary>
        /// Unique identifier for the expense type
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExpensesTypeId { get; set; }

        /// <summary>
        /// Foreign key to the Account this expense type belongs to
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Name of the expense type
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ExpensesTypeName { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool AllBranches { get; set; } = true;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active";

        /// <summary>
        /// User ID who created this expense type
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Date and time when expense type was created (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when expense type was last updated
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Foreign key to the Firm (optional)
        /// </summary>
        public Guid? FirmId { get; set; }

        // Navigation properties
        /// <summary>
        /// The account this expense type belongs to
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;

        /// <summary>
        /// The firm this expense type belongs to (optional)
        /// </summary>
        [ForeignKey("FirmId")]
        public virtual Firm? Firm { get; set; }

        public virtual ICollection<ExpensesTypeBranch> ExpenseBranches { get; set; } = new List<ExpensesTypeBranch>();
    }
}
