using System.ComponentModel.DataAnnotations;

namespace UserRegister.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-zА-Яа-я]{3,}$", ErrorMessage = "Имя должно содержать только буквы и быть не менее 3 символов.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Неверный формат Email.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "Пароль должен содержать буквы, цифры и быть не менее 8 символов.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
