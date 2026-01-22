using System;

namespace TheBeautyHubCore.DTOs
{
    public class PlansDto
    {
        public Guid PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string? PlanDescription { get; set; }
        public decimal PlanCost { get; set; }
        public bool IsPlanActive { get; set; }
        public string? PlanAppliedTo { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CreatePlanDto
    {
        public string PlanName { get; set; } = string.Empty;
        public string? PlanDescription { get; set; }
        public decimal PlanCost { get; set; }
        public bool IsPlanActive { get; set; }
        public string? PlanAppliedTo { get; set; }
    }

    public class UpdatePlanDto
    {
        public Guid PlanId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string? PlanDescription { get; set; }
        public decimal PlanCost { get; set; }
        public bool IsPlanActive { get; set; }
        public string? PlanAppliedTo { get; set; }
    }
}
