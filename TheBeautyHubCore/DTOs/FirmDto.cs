using System;

namespace TheBeautyHubCore.DTOs
{
    public class FirmDto
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

    public class CreateFirmDto
    {
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
    }

    public class UpdateFirmDto
    {
        public Guid FirmId { get; set; }
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
    }
}
