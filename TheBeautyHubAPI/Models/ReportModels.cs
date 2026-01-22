using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateReportRequest
    {
        [Required]
        [StringLength(200)]
        public string ReportName { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
    }

    public class UpdateReportRequest
    {
        [Required]
        [StringLength(200)]
        public string ReportName { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
    }

    public class ReportResponse
    {
        public Guid ReportId { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
