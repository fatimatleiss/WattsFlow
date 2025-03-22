using System.ComponentModel.DataAnnotations;

namespace WattsFlowProject.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
