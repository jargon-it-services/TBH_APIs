using System;

namespace TheBeautyHubAPI.Models
{
    /// <summary>
    /// Request model for creating a new service.
    /// </summary>
    public class CreateServicesRequest
    {
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceDescription { get; set; }
        public decimal ServicePrice { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsIncentiveApplicable { get; set; }
        public decimal? IncentiveAmount { get; set; }
        public int? IncentivePercentage { get; set; }
        public Guid? CreatedBy { get; set; }
    }

    /// <summary>
    /// Request model for updating an existing service.
    /// </summary>
    public class UpdateServicesRequest
    {
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceDescription { get; set; }
        public decimal ServicePrice { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsIncentiveApplicable { get; set; }
        public decimal? IncentiveAmount { get; set; }
        public int? IncentivePercentage { get; set; }
    }

    /// <summary>
    /// Response model for service information.
    /// </summary>
    public class ServicesResponse
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceDescription { get; set; }
        public decimal ServicePrice { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public bool IsIncentiveApplicable { get; set; }
        public decimal? IncentiveAmount { get; set; }
        public int? IncentivePercentage { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
