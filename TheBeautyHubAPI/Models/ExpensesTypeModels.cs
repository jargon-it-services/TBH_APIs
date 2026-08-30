using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheBeautyHubAPI.Models
{
    public class ExpenseListItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("all_branches")]
        public bool AllBranches { get; set; }

        [JsonPropertyName("branch_names")]
        public List<string> BranchNames { get; set; } = new();

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class ExpenseListDataResponse
    {
        [JsonPropertyName("expenses")]
        public IEnumerable<ExpenseListItemResponse> Expenses { get; set; } = Array.Empty<ExpenseListItemResponse>();
    }

    public class ExpenseBranchItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ExpenseDetailResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("all_branches")]
        public bool AllBranches { get; set; }

        [JsonPropertyName("branches")]
        public List<ExpenseBranchItemResponse> Branches { get; set; } = new();

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class ExpenseSavedDataResponse
    {
        [JsonPropertyName("saved")]
        public bool Saved { get; set; }
    }

    public class ExpenseDeletedDataResponse
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }

    public class SaveExpenseRequest
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("all_branches")]
        public bool? AllBranches { get; set; }

        [JsonPropertyName("branches")]
        public List<Guid>? Branches { get; set; }

        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
