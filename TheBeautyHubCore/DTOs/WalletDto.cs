using System;

namespace TheBeautyHubCore.DTOs
{
    /// <summary>
    /// DTO for Wallet entity.
    /// Used for retrieving wallet information.
    /// </summary>
    public class WalletDto
    {
        public Guid WalletId { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string WalletType { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating a new wallet.
    /// </summary>
    public class CreateWalletDto
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string WalletType { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing wallet.
    /// </summary>
    public class UpdateWalletDto
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string WalletType { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
    }
}
