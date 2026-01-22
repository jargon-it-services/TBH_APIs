using System;

namespace TheBeautyHubCore.DTOs
{
    /// <summary>
    /// DTO for TransactionType entity.
    /// Used for retrieving transaction type information.
    /// </summary>
    public class TransactionTypeDto
    {
        public Guid TransactionTypeId { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsTransactionTypeActive { get; set; }
    }

    /// <summary>
    /// DTO for creating a new transaction type.
    /// </summary>
    public class CreateTransactionTypeDto
    {
        public string Type { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for updating an existing transaction type.
    /// </summary>
    public class UpdateTransactionTypeDto
    {
        public Guid TransactionTypeId { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsTransactionTypeActive { get; set; }
    }
}
