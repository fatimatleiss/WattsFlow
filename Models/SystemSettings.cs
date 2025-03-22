using System.ComponentModel.DataAnnotations;

namespace WattsFlowProject.Models
{
    public class SystemSettings
    {
        [Key]
        public int Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "يجب أن يكون سعر الكيلوواط قيمة إيجابية.")]
        [Required(ErrorMessage = "سعر الكيلوواط مطلوب.")]
        public decimal KwPrice { get; set; }

        [Required(ErrorMessage = "سعر الليرة مطلوب.")]
        [Range(0, double.MaxValue, ErrorMessage = "يجب أن يكون سعر الليرة قيمة إيجابية.")]
        public decimal LiraRate { get; set; }
        public string? Title { get; set; }
        
    }
}
