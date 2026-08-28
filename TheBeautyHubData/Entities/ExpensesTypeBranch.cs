using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBeautyHubData.Entities
{
    [Table("ExpensesTypeBranch")]
    public class ExpensesTypeBranch
    {
        [Required]
        public Guid ExpensesTypeId { get; set; }

        [Required]
        public Guid BranchId { get; set; }

        [ForeignKey("ExpensesTypeId")]
        public virtual ExpensesType ExpensesType { get; set; } = null!;

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;
    }
}
