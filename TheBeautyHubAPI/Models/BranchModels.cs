using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace TheBeautyHubAPI.Models
{
    public class BranchListItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [JsonPropertyName("branch_type")]
        public string BranchType { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("logo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Logo { get; set; }
    }

    public class BranchListDataResponse
    {
        [JsonPropertyName("branches")]
        public IEnumerable<BranchListItemResponse> Branches { get; set; } = Array.Empty<BranchListItemResponse>();
    }

    public class BranchServiceItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class BranchEmployeeItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("photo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Photo { get; set; }
    }

    public class BranchDetailResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("address_line1")]
        public string AddressLine1 { get; set; } = string.Empty;

        [JsonPropertyName("address_line2")]
        public string AddressLine2 { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("pincode")]
        public string Pincode { get; set; } = string.Empty;

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("branch_type")]
        public string BranchType { get; set; } = string.Empty;

        [JsonPropertyName("opening_time")]
        public string OpeningTime { get; set; } = string.Empty;

        [JsonPropertyName("closing_time")]
        public string ClosingTime { get; set; } = string.Empty;

        [JsonPropertyName("weekly_off")]
        public string WeeklyOff { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Longitude { get; set; }

        [JsonPropertyName("maps_link")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MapsLink { get; set; }

        [JsonPropertyName("services")]
        public List<BranchServiceItemResponse> Services { get; set; } = new();

        [JsonPropertyName("employees")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<BranchEmployeeItemResponse>? Employees { get; set; }

        [JsonPropertyName("logo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Logo { get; set; }
    }

    public class BranchSavedDataResponse
    {
        [JsonPropertyName("saved")]
        public bool Saved { get; set; }
    }

    public class SaveBranchRequest
    {
        [JsonPropertyName("account_id")]
        public Guid? AccountId { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("address_line1")]
        public string AddressLine1 { get; set; } = string.Empty;

        [JsonPropertyName("address_line2")]
        public string? AddressLine2 { get; set; }

        [Required]
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("pincode")]
        public string Pincode { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("branch_type")]
        public string BranchType { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("opening_time")]
        public string OpeningTime { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("closing_time")]
        public string ClosingTime { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("weekly_off")]
        public string WeeklyOff { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("services")]
        public List<Guid>? Services { get; set; }

        [JsonPropertyName("service_id")]
        public Guid? ServiceId { get; set; }

        [JsonPropertyName("service_ids")]
        public List<Guid>? ServiceIds { get; set; }

        [JsonPropertyName("latitude")]
        public decimal? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public decimal? Longitude { get; set; }

        [JsonPropertyName("maps_link")]
        public string? MapsLink { get; set; }

        [JsonPropertyName("remove_logo")]
        public bool RemoveLogo { get; set; }

        [JsonIgnore]
        public IFormFile? Logo { get; set; }
    }
}
