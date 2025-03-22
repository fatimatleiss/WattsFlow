using System.ComponentModel.DataAnnotations;
using WattsFlowProject.Models;

namespace WattsFlowProject.ViewModels
{
    public class CustomerTrafficViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        public virtual List<Customer> Customers { get; set; } = new List<Customer>();

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

        public double Consumption { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "KW Price must be a positive number.")]
        public decimal KwPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Lira Rate must be a positive number.")]
        public decimal LiraRate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }   // Auto-calculated in USD

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Paid amount must be a positive number.")]
        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }

        public decimal PriceInLira { get; set; }

    }
}
