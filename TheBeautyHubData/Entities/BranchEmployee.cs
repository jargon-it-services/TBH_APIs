using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    /// <summary>
    /// Junction between a branch and an assigned staff user.
    /// </summary>
    [Table("BranchEmployee")]
    public class BranchEmployee
    {
        [Required]
        public Guid BranchId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [StringLength(500)]
        public string? Photo { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
