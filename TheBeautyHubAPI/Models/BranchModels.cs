using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    /// <summary>
    /// Create/update branch payload (API_023 / API_024).
    /// Required: name, address_line1, city, state, pincode, mobile, email, branch_type, opening_time, closing_time, weekly_off, status.
    /// Optional: address_line2, services / service_id / service_ids, latitude, longitude, maps_link, logo, remove_logo.
    /// Account is taken from the AuthCenter access token, not from the body.
    /// </summary>
    public class SaveBranchRequest
    {
        [Required]
        [FromForm(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "address_line1")]
        [JsonPropertyName("address_line1")]
        public string AddressLine1 { get; set; } = string.Empty;

        [FromForm(Name = "address_line2")]
        [JsonPropertyName("address_line2")]
        public string? AddressLine2 { get; set; }

        [Required]
        [FromForm(Name = "city")]
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "state")]
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "pincode")]
        [JsonPropertyName("pincode")]
        public string Pincode { get; set; } = string.Empty;

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
        [FromForm(Name = "branch_type")]
        [JsonPropertyName("branch_type")]
        public string BranchType { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "opening_time")]
        [JsonPropertyName("opening_time")]
        public string OpeningTime { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "closing_time")]
        [JsonPropertyName("closing_time")]
        public string ClosingTime { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "weekly_off")]
        [JsonPropertyName("weekly_off")]
        public string WeeklyOff { get; set; } = string.Empty;

        [Required]
        [FromForm(Name = "status")]
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [FromForm(Name = "services")]
        [JsonPropertyName("services")]
        public List<Guid>? Services { get; set; }

        [FromForm(Name = "service_id")]
        [JsonPropertyName("service_id")]
        public Guid? ServiceId { get; set; }

        [FromForm(Name = "service_ids")]
        [JsonPropertyName("service_ids")]
        public List<Guid>? ServiceIds { get; set; }

        [FromForm(Name = "latitude")]
        [JsonPropertyName("latitude")]
        public decimal? Latitude { get; set; }

        [FromForm(Name = "longitude")]
        [JsonPropertyName("longitude")]
        public decimal? Longitude { get; set; }

        [FromForm(Name = "maps_link")]
        [JsonPropertyName("maps_link")]
        public string? MapsLink { get; set; }

        [FromForm(Name = "remove_logo")]
        [JsonPropertyName("remove_logo")]
        public bool RemoveLogo { get; set; }

        [FromForm(Name = "logo")]
        [JsonIgnore]
        public IFormFile? Logo { get; set; }
    }
}
