using System;
using System.Collections.Generic;

namespace TheBeautyHubCore.DTOs
{
    public class ServiceCatalogItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public class ServiceListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ApplicableGender { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal CustomerPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Photo { get; set; }
    }

    public class ServiceBranchItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ServiceDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string ApplicableGender { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal CustomerPrice { get; set; }
        public decimal MaterialCost { get; set; }
        public string CommissionType { get; set; } = string.Empty;
        public decimal CommissionValue { get; set; }
        public decimal OtherCost { get; set; }
        public bool HomeServiceAvailable { get; set; }
        public decimal? HomeVisitCharges { get; set; }
        public decimal? ServiceRadiusKm { get; set; }
        public decimal? ExtraChargePerKm { get; set; }
        public bool AllBranches { get; set; }
        public List<ServiceBranchItemDto> Branches { get; set; } = new();
        public string? Photo { get; set; }
    }

    public class SaveServiceDto
    {
        public Guid AccountId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string ApplicableGender { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal CustomerPrice { get; set; }
        public decimal MaterialCost { get; set; }
        public string CommissionType { get; set; } = string.Empty;
        public decimal CommissionValue { get; set; }
        public decimal OtherCost { get; set; }
        public bool HomeServiceAvailable { get; set; }
        public decimal? HomeVisitCharges { get; set; }
        public decimal? ServiceRadiusKm { get; set; }
        public decimal? ExtraChargePerKm { get; set; }
        public bool AllBranches { get; set; }
        public List<Guid>? Branches { get; set; }
        public string? Photo { get; set; }
        public bool RemovePhoto { get; set; }
        public bool HasNewPhoto { get; set; }
    }
}
