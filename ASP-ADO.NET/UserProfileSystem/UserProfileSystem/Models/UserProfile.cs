using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserProfileSystem.Models
{
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }

        // Навигационное свойство для отношения один к одному
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}