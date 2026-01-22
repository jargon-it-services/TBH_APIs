using System;

namespace TheBeautyHubCore.DTOs
{
    /// <summary>
    /// DTO for ExpensesType entity.
    /// Used for retrieving expenses type information.
    /// </summary>
    public class ExpensesTypeDto
    {
        public Guid ExpensesTypeId { get; set; }
        public Guid AccountId { get; set; }
        public string ExpensesTypeName { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? FirmId { get; set; }
        public bool? IsDeleted { get; set; }
    }

    /// <summary>
    /// DTO for creating a new expenses type.
    /// </summary>
    public class CreateExpensesTypeDto
    {
        public Guid AccountId { get; set; }
        public string ExpensesTypeName { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
        public Guid? FirmId { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing expenses type.
    /// </summary>
    public class UpdateExpensesTypeDto
    {
        public Guid ExpensesTypeId { get; set; }
        public string ExpensesTypeName { get; set; } = string.Empty;
        public Guid? FirmId { get; set; }
    }
}
