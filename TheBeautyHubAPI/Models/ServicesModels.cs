using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace TheBeautyHubAPI.Models
{
    public class ServiceCatalogItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }

    public class ServiceCatalogDataResponse
    {
        [JsonPropertyName("services")]
        public IEnumerable<ServiceCatalogItemResponse> Services { get; set; } = Array.Empty<ServiceCatalogItemResponse>();
    }

    public class ServiceListItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("applicable_gender")]
        public string ApplicableGender { get; set; } = string.Empty;

        [JsonPropertyName("duration_minutes")]
        public int DurationMinutes { get; set; }

        [JsonPropertyName("customer_price")]
        public decimal CustomerPrice { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("photo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Photo { get; set; }
    }

    public class ServiceListDataResponse
    {
        [JsonPropertyName("services")]
        public IEnumerable<ServiceListItemResponse> Services { get; set; } = Array.Empty<ServiceListItemResponse>();
    }

    public class ServiceBranchItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ServiceDetailResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("duration_minutes")]
        public int DurationMinutes { get; set; }

        [JsonPropertyName("applicable_gender")]
        public string ApplicableGender { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("customer_price")]
        public decimal CustomerPrice { get; set; }

        [JsonPropertyName("material_cost")]
        public decimal MaterialCost { get; set; }

        [JsonPropertyName("commission_type")]
        public string CommissionType { get; set; } = string.Empty;

        [JsonPropertyName("commission_value")]
        public decimal CommissionValue { get; set; }

        [JsonPropertyName("other_cost")]
        public decimal OtherCost { get; set; }

        [JsonPropertyName("home_service_available")]
        public bool HomeServiceAvailable { get; set; }

        [JsonPropertyName("home_visit_charges")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? HomeVisitCharges { get; set; }

        [JsonPropertyName("service_radius_km")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? ServiceRadiusKm { get; set; }

        [JsonPropertyName("extra_charge_per_km")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? ExtraChargePerKm { get; set; }

        [JsonPropertyName("all_branches")]
        public bool AllBranches { get; set; }

        [JsonPropertyName("branches")]
        public List<ServiceBranchItemResponse> Branches { get; set; } = new();

        [JsonPropertyName("photo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Photo { get; set; }
    }

    public class ServiceSavedDataResponse
    {
        [JsonPropertyName("saved")]
        public bool Saved { get; set; }
    }

    public class ServiceDeletedDataResponse
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }

    public class SaveServiceRequest
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("duration_minutes")]
        public int? DurationMinutes { get; set; }

        [Required]
        [JsonPropertyName("applicable_gender")]
        public string ApplicableGender { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("customer_price")]
        public decimal? CustomerPrice { get; set; }

        [Required]
        [JsonPropertyName("material_cost")]
        public decimal? MaterialCost { get; set; }

        [Required]
        [JsonPropertyName("commission_type")]
        public string CommissionType { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("commission_value")]
        public decimal? CommissionValue { get; set; }

        [Required]
        [JsonPropertyName("other_cost")]
        public decimal? OtherCost { get; set; }

        [Required]
        [JsonPropertyName("home_service_available")]
        public bool? HomeServiceAvailable { get; set; }

        [JsonPropertyName("home_visit_charges")]
        public decimal? HomeVisitCharges { get; set; }

        [JsonPropertyName("service_radius_km")]
        public decimal? ServiceRadiusKm { get; set; }

        [JsonPropertyName("extra_charge_per_km")]
        public decimal? ExtraChargePerKm { get; set; }

        [Required]
        [JsonPropertyName("all_branches")]
        public bool? AllBranches { get; set; }

        [JsonPropertyName("branches")]
        public List<Guid>? Branches { get; set; }

        [JsonPropertyName("remove_photo")]
        public bool RemovePhoto { get; set; }

        [JsonIgnore]
        public IFormFile? Photo { get; set; }
    }
}
