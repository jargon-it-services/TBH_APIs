using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents a Wallet in the system.
    /// Wallets store monetary amounts for different purposes (ReferralBonus, Promotional, Cashback).
    /// </summary>
    [Table("Wallet")]
    public class Wallet
    {
        /// <summary>
        /// Unique identifier for the wallet
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WalletId { get; set; }

        /// <summary>
        /// Foreign key to the Account this wallet belongs to
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Wallet balance amount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;

        /// <summary>
        /// Type of wallet: ReferralBonus, Promotional, or Cashback
        /// </summary>
        [Required]
        [StringLength(30)]
        public string WalletType { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the wallet balance has been used
        /// </summary>
        [Required]
        public bool IsUsed { get; set; } = false;

        /// <summary>
        /// Date and time when wallet was created (UTC)
        /// </summary>
        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        /// <summary>
        /// The account this wallet belongs to
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } = null!;
    }
}
