using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreatePartnerRequest
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Type { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(20)]
        public string? Mobile { get; set; }
        
        [EmailAddress]
        [StringLength(256)]
        public string? Email { get; set; }
        
        public DateTime? DateofBirth { get; set; }
        
        [StringLength(20)]
        public string? Gender { get; set; }
        
        public Guid? AccountId { get; set; }
    }

    public class UpdatePartnerRequest
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Type { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(20)]
        public string? Mobile { get; set; }
        
        [EmailAddress]
        [StringLength(256)]
        public string? Email { get; set; }
        
        public DateTime? DateofBirth { get; set; }
        
        [StringLength(20)]
        public string? Gender { get; set; }
    }

    public class PartnerResponse
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
}
