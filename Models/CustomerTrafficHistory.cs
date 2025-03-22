using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WattsFlowProject.Models
{
    public class CustomerTrafficHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        [Required]
        public double PreviousTraffic { get; set; }

        [Required]
        public DateTime PreviousTrafficDate { get; set; }

        [Required]
        public double CurrentTraffic { get; set; }

        [Required]
        public DateTime CurrentTrafficDate { get; set; }

        public double Consumption { get; set; }

        [Required]
        public decimal PriceUSD { get; set; }

        [Required]
        public decimal PriceLBP { get; set; }

        public decimal? PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}