using System;
using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateFirmDetailsRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid FirmId { get; set; }
    }

    public class UpdateFirmDetailsRequest
    {
        [Required]
        public Guid FirmDetailsId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid FirmId { get; set; }
    }

    public class FirmDetailsResponse
    {
        public Guid FirmDetailsId { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FirmId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
