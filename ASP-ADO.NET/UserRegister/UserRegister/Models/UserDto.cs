using System.ComponentModel.DataAnnotations;

namespace UserRegister.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-zА-Яа-я]{3,}$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$")]
        public string Password { get; set; }
    }

}
