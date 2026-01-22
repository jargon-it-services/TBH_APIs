using System;
using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateFirmRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        [StringLength(200)]
        public string FirmName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? ContactNumber { get; set; }

        [StringLength(20)]
        public string? AlternateContactNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? EmailId { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? AlternateEmailId { get; set; }

        [StringLength(500)]
        public string? AddressLine1 { get; set; }

        [StringLength(500)]
        public string? AddressLine2 { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }
    }

    public class UpdateFirmRequest
    {
        [Required]
        public Guid FirmId { get; set; }

        [Required]
        [StringLength(200)]
        public string FirmName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? ContactNumber { get; set; }

        [StringLength(20)]
        public string? AlternateContactNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? EmailId { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? AlternateEmailId { get; set; }

        [StringLength(500)]
        public string? AddressLine1 { get; set; }

        [StringLength(500)]
        public string? AddressLine2 { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }
    }

    public class FirmResponse
    {
        public Guid FirmId { get; set; }
        public Guid AccountId { get; set; }
        public string FirmName { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
        public string? AlternateContactNumber { get; set; }
        public string? EmailId { get; set; }
        public string? AlternateEmailId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
