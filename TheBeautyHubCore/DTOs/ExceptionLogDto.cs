namespace TheBeautyHubCore.DTOs
{
    public class ExceptionLogDto
    {
        public long Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? DeviceName { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateExceptionLogDto
    {
        public string Type { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? DeviceName { get; set; }
        public Guid? UserId { get; set; }
    }
}
