using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreateTransactionRequest
    {
        public string Status { get; set; } = "Draft";
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [Required]
        public Guid AccountId { get; set; }
        
        public Guid? FirmId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }

    public class UpdateTransactionRequest
    {
        public string Status { get; set; } = "Draft";
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public DateTime? PostedDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }

    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public string Status { get; set; } = "Draft";
        public decimal TotalAmount { get; set; }
        public Guid AccountId { get; set; }
        public Guid? FirmId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
