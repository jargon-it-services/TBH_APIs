using System;

namespace TheBeautyHubAPI.Models
{
    /// <summary>
    /// Request model for creating a new expenses type.
    /// </summary>
    public class CreateExpensesTypeRequest
    {
        public Guid AccountId { get; set; }
        public string ExpensesTypeName { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
        public Guid? FirmId { get; set; }
    }

    /// <summary>
    /// Request model for updating an existing expenses type.
    /// </summary>
    public class UpdateExpensesTypeRequest
    {
        public string ExpensesTypeName { get; set; } = string.Empty;
        public Guid? FirmId { get; set; }
    }

    /// <summary>
    /// Response model for expenses type information.
    /// </summary>
    public class ExpensesTypeResponse
    {
        public Guid ExpensesTypeId { get; set; }
        public Guid AccountId { get; set; }
        public string ExpensesTypeName { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? FirmId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
