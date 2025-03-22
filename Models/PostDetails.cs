using System.ComponentModel.DataAnnotations;

namespace WattsFlowProject.Models
{
    public class PostDetails
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        public int PostNumber { get; set; }
        [Required]
        public string? PostAddress { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();  

    }
}
