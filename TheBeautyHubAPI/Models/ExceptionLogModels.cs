using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateExceptionLogRequest
    {
        [Required]
        [StringLength(100)]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        public string ErrorMessage { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? DeviceName { get; set; }
        
        public Guid? UserId { get; set; }
    }

    public class ExceptionLogResponse
    {
        public long Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string? DeviceName { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
