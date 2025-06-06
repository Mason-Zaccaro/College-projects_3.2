using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public class User
    {
        [Required]
        [MinLength(3, ErrorMessage = "Имя пользователя должно содержать не менее 3 символов.")]
        [RegularExpression("^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Имя может содержать только буквы.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*[0-9]).+$", ErrorMessage = "Пароль должен содержать буквы и цифры.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
