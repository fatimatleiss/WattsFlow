using System.ComponentModel.DataAnnotations;

namespace WattsFlowProject.Models
{
    public class ExpenseType
    {
        [Key]
        public int ExpensesTypeId { get; set; }
        [Required]
        public string? ExpenseTypeName { get; set; }

        public bool IsDeleted { get; set; } = false;  

        public virtual ICollection<Expenses> Expenses { get; set; } = new List<Expenses>();


    }
}
