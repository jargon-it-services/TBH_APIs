using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateReportForAccountRequest
    {
        [Required]
        public Guid ReportId { get; set; }
        
        [Required]
        public Guid AccountId { get; set; }
        
        public bool IsActive { get; set; } = true;
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateReportForAccountRequest
    {
        public bool IsActive { get; set; }
    }

    public class ReportForAccountResponse
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public Guid AccountId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
