namespace TheBeautyHubCore.DTOs
{
    public class ReportForAccountDto
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public Guid AccountId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class CreateReportForAccountDto
    {
        public Guid ReportId { get; set; }
        public Guid AccountId { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid? CreatedBy { get; set; }
    }

    public class UpdateReportForAccountDto
    {
        public bool IsActive { get; set; }
    }
}
