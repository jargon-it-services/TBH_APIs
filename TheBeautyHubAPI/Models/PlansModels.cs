using System;
using System.ComponentModel.DataAnnotations;

namespace TheBeautyHubAPI.Models
{
    public class CreatePlanRequest
    {
        [Required]
        [StringLength(200)]
        public string PlanName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? PlanDescription { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PlanCost { get; set; }

        [Required]
        public bool IsPlanActive { get; set; }

        [StringLength(200)]
        public string? PlanAppliedTo { get; set; }
    }

    public class UpdatePlanRequest
    {
        [Required]
        public Guid PlanId { get; set; }

        [Required]
        [StringLength(200)]
        public string PlanName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? PlanDescription { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PlanCost { get; set; }

        [Required]
        public bool IsPlanActive { get; set; }

        [StringLength(200)]
        public string? PlanAppliedTo { get; set; }
    }

    public class PlanResponse
    {
        public Guid PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string? PlanDescription { get; set; }
        public decimal PlanCost { get; set; }
        public bool IsPlanActive { get; set; }
        public string? PlanAppliedTo { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
