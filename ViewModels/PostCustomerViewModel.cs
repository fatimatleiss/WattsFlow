using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using WattsFlowProject.Models;

namespace WattsFlowProject.ViewModels
{
    public class PostCustomerViewModel
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? FatherName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Phone number must contain exactly 8 digits.")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Building { get; set; }

        [Required]
        public string? Floor { get; set; }

        [Required]
        public string? Side { get; set; }

        [Required]
        [ForeignKey("PostId")]
        public int PostId { get; set; }

        [Required]
        public double CircuitBreakerPower { get; set; }

        [Required]
        public decimal FixedPayment { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeactivationDate { get; set; }

        public List<SelectListItem> posts { get; set; } = new List<SelectListItem>();
    }
}
