using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Junction between a branch and a catalog service.
    /// </summary>
    [Table("BranchService")]
    public class BranchService
    {
        [Required]
        public Guid BranchId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;

        [ForeignKey("ServiceId")]
        public virtual Services Service { get; set; } = null!;
    }
}
