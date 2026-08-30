using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TheBeautyHubAPI.Models
{
    public class AccountSummaryDataResponse
    {
        [JsonPropertyName("total_branches")]
        public int TotalBranches { get; set; }

        [JsonPropertyName("total_staff")]
        public int TotalStaff { get; set; }

        [JsonPropertyName("total_services")]
        public int TotalServices { get; set; }

        [JsonPropertyName("total_expenses")]
        public int TotalExpenses { get; set; }

        [JsonPropertyName("total_salary_rules")]
        public int TotalSalaryRules { get; set; }
    }

    public class FeatureLockDataResponse
    {
        [JsonPropertyName("feature_lock")]
        public List<string> FeatureLock { get; set; } = new();
    }
}
