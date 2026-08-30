using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [FromForm(Name = "full_name")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "mobile")]
        [JsonPropertyName("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [FromForm(Name = "email")]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "gender")]
        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "aadhaar_number")]
        [JsonPropertyName("aadhaar_number")]
        public string AadhaarNumber { get; set; } = string.Empty;

        [FromForm(Name = "employee_code")]
        [JsonPropertyName("employee_code")]
        public string? EmployeeCode { get; set; }

        [FromForm(Name = "joining_date")]
        [JsonPropertyName("joining_date")]
        public string? JoiningDate { get; set; }

        [Required]
        [FromForm(Name = "designation")]
        [JsonPropertyName("designation")]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "specialist")]
        [JsonPropertyName("specialist")]
        public string Specialist { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "branch_id")]
        [JsonPropertyName("branch_id")]
        public Guid BranchId { get; set; }

        [Required]
        [FromForm(Name = "salary_rule_id")]
        [JsonPropertyName("salary_rule_id")]
        public Guid SalaryRuleId { get; set; }

        [Required]
        [FromForm(Name = "status")]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [FromForm(Name = "allow_app_login")]
        [JsonPropertyName("allow_app_login")]
        public bool AllowAppLogin { get; set; }

        [FromForm(Name = "app_role")]
        [JsonPropertyName("app_role")]
        public string? AppRole { get; set; }

        [FromForm(Name = "username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [FromForm(Name = "remove_photo")]
        [JsonPropertyName("remove_photo")]
        public bool RemovePhoto { get; set; }

        [FromForm(Name = "remove_aadhaar_card")]
        [JsonPropertyName("remove_aadhaar_card")]
        public bool RemoveAadhaarCard { get; set; }

        [FromForm(Name = "photo")]
        [JsonIgnore]
        public IFormFile? Photo { get; set; }

        [FromForm(Name = "aadhaar_card")]
        [JsonIgnore]
        public IFormFile? AadhaarCard { get; set; }
    }

    public class SalaryRuleCatalogItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }

    public class SalaryRuleCatalogDataResponse
    {
        [JsonPropertyName("salary_rules")]
        public IEnumerable<SalaryRuleCatalogItemResponse> SalaryRules { get; set; } = Array.Empty<SalaryRuleCatalogItemResponse>();
    }

    public class SalaryRuleListItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("salary_type")]
        public string SalaryType { get; set; } = string.Empty;

        [JsonPropertyName("fixed_salary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? FixedSalary { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class SalaryRuleListDataResponse
    {
        [JsonPropertyName("salary_rules")]
        public IEnumerable<SalaryRuleListItemResponse> SalaryRules { get; set; } = Array.Empty<SalaryRuleListItemResponse>();
    }

    public class SalaryRuleDetailResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("salary_type")]
        public string SalaryType { get; set; } = string.Empty;

        [JsonPropertyName("fixed_salary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? FixedSalary { get; set; }

        [JsonPropertyName("monthly_target")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? MonthlyTarget { get; set; }

        [JsonPropertyName("target_bonus")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? TargetBonus { get; set; }

        [JsonPropertyName("allow_advance_recovery")]
        public bool AllowAdvanceRecovery { get; set; }

        [JsonPropertyName("max_recovery_per_month")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? MaxRecoveryPerMonth { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class SalaryRuleSavedDataResponse
    {
        [JsonPropertyName("saved")]
        public bool Saved { get; set; }
    }

    public class SalaryRuleDeletedDataResponse
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }

    public class SaveSalaryRuleRequest
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("salary_type")]
        public string SalaryType { get; set; } = string.Empty;

        [JsonPropertyName("fixed_salary")]
        public decimal? FixedSalary { get; set; }

        [JsonPropertyName("monthly_target")]
        public decimal? MonthlyTarget { get; set; }

        [JsonPropertyName("target_bonus")]
        public decimal? TargetBonus { get; set; }

        [Required]
        [JsonPropertyName("allow_advance_recovery")]
        public bool? AllowAdvanceRecovery { get; set; }

        [JsonPropertyName("max_recovery_per_month")]
        public decimal? MaxRecoveryPerMonth { get; set; }

        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
