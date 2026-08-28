using System;
using System.Collections.Generic;

namespace TheBeautyHubCore.DTOs
{
    public class StaffFormBranchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class StaffFormSalaryRuleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public class StaffFormConfigDto
    {
        public List<StaffFormBranchDto> Branches { get; set; } = new();
        public List<StaffFormSalaryRuleDto> SalaryRules { get; set; } = new();
        public List<string> Specialists { get; set; } = new();
    }

    public class StaffListItemDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Specialist { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Photo { get; set; }
    }

    public class StaffDetailDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string AadhaarNumber { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string JoiningDate { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Specialist { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public Guid SalaryRuleId { get; set; }
        public string SalaryRuleName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool AllowAppLogin { get; set; }
        public string AppRole { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string? AadhaarCardUrl { get; set; }
    }

    public class SaveStaffDto
    {
        public Guid AccountId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string AadhaarNumber { get; set; } = string.Empty;
        public string? EmployeeCode { get; set; }
        public string? JoiningDate { get; set; }
        public string Designation { get; set; } = string.Empty;
        public string Specialist { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public Guid SalaryRuleId { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool AllowAppLogin { get; set; }
        public string? AppRole { get; set; }
        public string? Username { get; set; }
        public string? Photo { get; set; }
        public string? AadhaarCardUrl { get; set; }
        public bool RemovePhoto { get; set; }
        public bool RemoveAadhaarCard { get; set; }
        public bool HasNewPhoto { get; set; }
        public bool HasNewAadhaarCard { get; set; }
    }
}
