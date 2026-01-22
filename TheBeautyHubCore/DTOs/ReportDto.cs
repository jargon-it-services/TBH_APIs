namespace TheBeautyHubCore.DTOs
{
    public class ReportDto
    {
        public Guid ReportId { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class CreateReportDto
    {
        public string ReportName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateReportDto
    {
        public string ReportName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
