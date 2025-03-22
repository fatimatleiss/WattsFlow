using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WattsFlowProject.Models
{
    public class CustomerTraffic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Previous traffic must be a positive number.")]
        public double PreviousTraffic { get; set; } 
         
        [Required]
        public DateTime PreviousTrafficDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Current traffic must be a positive number.")]
        public double CurrentTraffic { get; set; }

        [Required]
        public DateTime CurrentTrafficDate { get; set; }

        public double Consumption { get; set; } // Add { get; set; }
        public decimal RemainingAmount { get; set; } // Add { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal PriceUSD { get; set; }
        [Required]
        public decimal PriceLBP { get; set; }
        [Required]
        public decimal? PaidAmount { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

    }
}
