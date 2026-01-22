using System;

namespace TheBeautyHubAPI.Models
{
    /// <summary>
    /// Request model for creating a new transaction type.
    /// </summary>
    public class CreateTransactionTypeRequest
    {
        public string TransactionType { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for updating an existing transaction type.
    /// </summary>
    public class UpdateTransactionTypeRequest
    {
        public string TransactionType { get; set; } = string.Empty;
        public bool IsTransactionTypeActive { get; set; }
    }

    /// <summary>
    /// Response model for transaction type information.
    /// </summary>
    public class TransactionTypeResponse
    {
        public Guid TransactionTypeId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public bool IsTransactionTypeActive { get; set; }
    }
}
