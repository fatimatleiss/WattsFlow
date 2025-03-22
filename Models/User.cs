using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WattsFlowProject.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty; 

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public string RoleName => Role?.RoleName; 

        public bool IsDeleted { get; set; } = false;
    }
}
