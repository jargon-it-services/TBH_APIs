using System;

namespace TheBeautyHubAPI.Models
{
    /// <summary>
    /// Request model for creating a new wallet.
    /// </summary>
    public class CreateWalletRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string WalletType { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
    }

    /// <summary>
    /// Request model for updating an existing wallet.
    /// </summary>
    public class UpdateWalletRequest
    {
        public decimal Amount { get; set; }
        public string WalletType { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
    }

    /// <summary>
    /// Response model for wallet information.
    /// </summary>
    public class WalletResponse
    {
        public Guid WalletId { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string WalletType { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
