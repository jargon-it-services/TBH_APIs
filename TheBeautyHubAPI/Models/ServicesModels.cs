using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBeautyHubAPI.Helpers;

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

    /// <summary>
    /// Create/update service payload (API_028 / API_029).
    /// Create still requires catalog fields (validated in the service layer).
    /// Status-only update is allowed: { "status": "active" } or { "status": "inactive" }.
    /// Extra fields such as app_name and platform are ignored.
    /// </summary>
    public class SaveServiceRequest
    {
        [FromForm(Name = "name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [FromForm(Name = "description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [FromForm(Name = "category")]
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [FromForm(Name = "duration_minutes")]
        [JsonPropertyName("duration_minutes")]
        public int? DurationMinutes { get; set; }

        [FromForm(Name = "applicable_gender")]
        [JsonPropertyName("applicable_gender")]
        public string? ApplicableGender { get; set; }

        [FromForm(Name = "type")]
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [FromForm(Name = "status")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [FromForm(Name = "customer_price")]
        [JsonPropertyName("customer_price")]
        public decimal? CustomerPrice { get; set; }

        [FromForm(Name = "material_cost")]
        [JsonPropertyName("material_cost")]
        public decimal? MaterialCost { get; set; }

        [FromForm(Name = "commission_type")]
        [JsonPropertyName("commission_type")]
        public string? CommissionType { get; set; }

        [FromForm(Name = "commission_value")]
        [JsonPropertyName("commission_value")]
        public decimal? CommissionValue { get; set; }

        [FromForm(Name = "other_cost")]
        [JsonPropertyName("other_cost")]
        public decimal? OtherCost { get; set; }

        [FromForm(Name = "home_service_available")]
        [JsonPropertyName("home_service_available")]
        public bool? HomeServiceAvailable { get; set; }

        [FromForm(Name = "home_visit_charges")]
        [JsonPropertyName("home_visit_charges")]
        public decimal? HomeVisitCharges { get; set; }

        [FromForm(Name = "service_radius_km")]
        [JsonPropertyName("service_radius_km")]
        public decimal? ServiceRadiusKm { get; set; }

        [FromForm(Name = "extra_charge_per_km")]
        [JsonPropertyName("extra_charge_per_km")]
        public decimal? ExtraChargePerKm { get; set; }

        [FromForm(Name = "all_branches")]
        [JsonPropertyName("all_branches")]
        public bool? AllBranches { get; set; }

        [FromForm(Name = "branches")]
        [JsonPropertyName("branches")]
        [ModelBinder(BinderType = typeof(GuidListModelBinder))]
        public List<Guid>? Branches { get; set; }

        [FromForm(Name = "branch_ids")]
        [JsonPropertyName("branch_ids")]
        [ModelBinder(BinderType = typeof(GuidListModelBinder))]
        public List<Guid>? BranchIds { get; set; }

        [FromForm(Name = "remove_photo")]
        [JsonPropertyName("remove_photo")]
        public bool RemovePhoto { get; set; }

        [FromForm(Name = "photo")]
        [JsonIgnore]
        public IFormFile? Photo { get; set; }
    }
}
