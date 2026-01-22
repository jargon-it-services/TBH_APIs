using System;

namespace TheBeautyHubCore.DTOs
{
    public class FirmDetailsDto
    {
        public Guid FirmDetailsId { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FirmId { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CreateFirmDetailsDto
    {
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FirmId { get; set; }
    }

    public class UpdateFirmDetailsDto
    {
        public Guid FirmDetailsId { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FirmId { get; set; }
    }
}
