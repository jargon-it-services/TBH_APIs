using System.Collections.Generic;

namespace TheBeautyHubCore.DTOs
{
    public class AccountSummaryDto
    {
        public int TotalBranches { get; set; }
        public int TotalStaff { get; set; }
        public int TotalServices { get; set; }
        public int TotalExpenses { get; set; }
        public int TotalSalaryRules { get; set; }
    }

    public class FeatureLockDto
    {
        public List<string> FeatureLock { get; set; } = new();
    }
}
