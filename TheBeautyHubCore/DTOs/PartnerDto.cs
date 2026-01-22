namespace TheBeautyHubCore.DTOs
{
    public class PartnerDto
    {
        public Guid PartnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePartnerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? AccountId { get; set; }
    }

    public class UpdatePartnerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? Gender { get; set; }
    }
}
