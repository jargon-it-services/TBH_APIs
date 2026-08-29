using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Represents an exception/error log entry.
    /// Stores application errors and exceptions for debugging and monitoring.
    /// </summary>
    [Table("ExceptionLogs")]
    public class ExceptionLog
    {
        /// <summary>
        /// Unique identifier for the exception log (auto-increment)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Type of exception
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Error message
        /// </summary>
        [Required]
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Device name or identifier
        /// </summary>
        [StringLength(100)]
        public string? DeviceName { get; set; }

        /// <summary>
        /// User ID associated with the exception (optional)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Date and time when exception occurred (UTC)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
