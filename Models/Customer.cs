using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WattsFlowProject.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الاسم الأول.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[\p{L}\d\s]+$", ErrorMessage = "يجب أن يحتوي الاسم الأول على حروف وليس أرقامًا فقط.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز الاسم الأول 50 حرفًا.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم الأب.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[\p{L}\d\s]+$", ErrorMessage = "يجب أن يحتوي اسم الأب على حروف وليس أرقامًا فقط.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم الأب 50 حرفًا.")]
        public string? FatherName { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم العائلة.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[\p{L}\d\s]+$", ErrorMessage = "يجب أن يحتوي اسم العائلة على حروف وليس أرقامًا فقط.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم العائلة 50 حرفًا.")]
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {FatherName} {LastName}".Trim();

        [Required(ErrorMessage = "الرجاء إدخال رقم الهاتف.")]
        [Display(Name = "رقم الهاتف")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "يجب أن يحتوي رقم الهاتف على 8 أرقام تمامًا.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال العنوان.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[\p{L}\d\s]+$", ErrorMessage = "يجب أن يحتوي العنوان على حروف وليس أرقامًا فقط.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز العنوان 100 حرف.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم البناء.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[\p{L}\d\s]+$", ErrorMessage = "يجب أن يحتوي اسم البناء على حروف وليس أرقامًا فقط.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم البناء 50 حرفًا.")]
        public string? Building { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الطابق.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "يجب أن يكون الطابق رقمًا صحيحًا.")]
        public string? Floor { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الجهة.")]
        [RegularExpression("^(Left|Right|Front|Back)$", ErrorMessage = "الرجاء إدخال قيمة صحيحة للجهة (Left, Right, Front, Back).")]
        public string? SideType { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار رقم العمود.")]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual PostDetails PostDetails { get; set; } = null!;

        [Required(ErrorMessage = "الرجاء إدخال قدرة القاطع الكهربائي.")]
        [Range(0.1, 100, ErrorMessage = "يجب أن تكون قدرة القاطع بين 0.1 و 100.")]
        public double CircuitBreakerPower { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال قيمة الدفع الثابت.")]
        [Range(0, double.MaxValue, ErrorMessage = "يجب أن تكون قيمة الدفع الثابت رقمًا موجبًا.")]
        public decimal FixedPayment { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeactivationDate { get; set; }

        public bool IsDeleted { get; set; } = false;
        public ICollection<CustomerTraffic> Traffics { get; set; } = new List<CustomerTraffic>();

        public ICollection<CustomerTrafficHistory> TrafficHistories { get; set; } = new List<CustomerTrafficHistory>();

    }
}
