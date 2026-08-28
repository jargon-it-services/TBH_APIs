using System;
using System.Collections.Generic;

namespace TheBeautyHubCore.DTOs
{
    public class BranchListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string BranchType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Logo { get; set; }
    }

    public class BranchServiceItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class BranchEmployeeItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Photo { get; set; }
    }

    public class BranchDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BranchType { get; set; } = string.Empty;
        public string OpeningTime { get; set; } = string.Empty;
        public string ClosingTime { get; set; } = string.Empty;
        public string WeeklyOff { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? MapsLink { get; set; }
        public List<BranchServiceItemDto> Services { get; set; } = new();
        public List<BranchEmployeeItemDto> Employees { get; set; } = new();
        public string? Logo { get; set; }
    }

    public class SaveBranchDto
    {
        public Guid? AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BranchType { get; set; } = string.Empty;
        public string OpeningTime { get; set; } = string.Empty;
        public string ClosingTime { get; set; } = string.Empty;
        public string WeeklyOff { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<Guid>? Services { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? MapsLink { get; set; }
        public string? Logo { get; set; }
        public bool RemoveLogo { get; set; }
        public bool HasNewLogo { get; set; }
        public Guid? CreatedBy { get; set; }
    }

    public class BranchSavedDto
    {
        public bool Saved { get; set; }
    }
}
