using System.ComponentModel.DataAnnotations;
using WattsFlowProject.Models;

namespace WattsFlowProject.ViewModels
{
    public class ExpensesViewModel
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [Required]
        [Display(Name = "Expense Type")]
        public int ExpensesTypeId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }

        public List<ExpenseType> types { get; set; } = new List<ExpenseType>();

    }
}
