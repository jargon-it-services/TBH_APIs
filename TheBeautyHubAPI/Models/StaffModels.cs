using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace TheBeautyHubAPI.Models
{
    public class StaffFormBranchResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
    }

    public class StaffFormSalaryRuleResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }

    public class StaffFormConfigDataResponse
    {
        [JsonPropertyName("branches")]
        public List<StaffFormBranchResponse> Branches { get; set; } = new();

        [JsonPropertyName("salary_rules")]
        public List<StaffFormSalaryRuleResponse> SalaryRules { get; set; } = new();

        [JsonPropertyName("specialists")]
        public List<string> Specialists { get; set; } = new();
    }

    public class StaffListItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("employee_code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [JsonPropertyName("designation")]
        public string Designation { get; set; } = string.Empty;

        [JsonPropertyName("specialist")]
        public string Specialist { get; set; } = string.Empty;

        [JsonPropertyName("branch_name")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("photo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Photo { get; set; }
    }

    public class StaffListDataResponse
    {
        [JsonPropertyName("staff")]
        public List<StaffListItemResponse> Staff { get; set; } = new();
    }

    public class StaffDetailResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonPropertyName("aadhaar_number")]
        public string AadhaarNumber { get; set; } = string.Empty;

        [JsonPropertyName("employee_code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [JsonPropertyName("joining_date")]
        public string JoiningDate { get; set; } = string.Empty;

        [JsonPropertyName("designation")]
        public string Designation { get; set; } = string.Empty;

        [JsonPropertyName("specialist")]
        public string Specialist { get; set; } = string.Empty;

        [JsonPropertyName("branch_id")]
        public Guid BranchId { get; set; }

        [JsonPropertyName("branch_name")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("salary_rule_id")]
        public Guid SalaryRuleId { get; set; }

        [JsonPropertyName("salary_rule_name")]
        public string SalaryRuleName { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("allow_app_login")]
        public bool AllowAppLogin { get; set; }

        [JsonPropertyName("app_role")]
        public string AppRole { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("photo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Photo { get; set; }

        [JsonPropertyName("aadhaar_card_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AadhaarCardUrl { get; set; }
    }

    public class NextEmployeeCodeDataResponse
    {
        [JsonPropertyName("employee_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EmployeeCode { get; set; }
    }

    public class StaffSavedDataResponse
    {
        [JsonPropertyName("saved")]
        public bool Saved { get; set; }
    }

    public class StaffDeletedDataResponse
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }

    public class SaveStaffRequest
    {
        [Required]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("aadhaar_number")]
        public string AadhaarNumber { get; set; } = string.Empty;

        [JsonPropertyName("employee_code")]
        public string? EmployeeCode { get; set; }

        [JsonPropertyName("joining_date")]
        public string? JoiningDate { get; set; }

        [Required]
        [JsonPropertyName("designation")]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("specialist")]
        public string Specialist { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("branch_id")]
        public Guid BranchId { get; set; }

        [Required]
        [JsonPropertyName("salary_rule_id")]
        public Guid SalaryRuleId { get; set; }

        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("allow_app_login")]
        public bool AllowAppLogin { get; set; }

        [JsonPropertyName("app_role")]
        public string? AppRole { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("remove_photo")]
        public bool RemovePhoto { get; set; }

        [JsonPropertyName("remove_aadhaar_card")]
        public bool RemoveAadhaarCard { get; set; }

        [JsonIgnore]
        public IFormFile? Photo { get; set; }

        [JsonIgnore]
        public IFormFile? AadhaarCard { get; set; }
    }
}
