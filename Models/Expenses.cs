using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WattsFlowProject.Models
{
    public class Expenses
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public int ExpensesTypeId { get; set; }

        [ForeignKey("ExpensesTypeId")]
        public virtual ExpenseType? ExpenseType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; } = false;  

    }
}
