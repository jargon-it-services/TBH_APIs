using System;
using System.Collections.Generic;

namespace TheBeautyHubCore.DTOs
{
    public class ExpenseListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool AllBranches { get; set; }
        public List<string> BranchNames { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }

    public class ExpenseBranchItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ExpenseDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool AllBranches { get; set; }
        public List<ExpenseBranchItemDto> Branches { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }

    public class SaveExpenseDto
    {
        public Guid AccountId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool AllBranches { get; set; }
        public List<Guid>? Branches { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
