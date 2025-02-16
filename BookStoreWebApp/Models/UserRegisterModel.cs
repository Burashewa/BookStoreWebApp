using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.Models
{
    public class UserRegisterModel
    {
        [Required]
        [StringLength(50,MinimumLength =4)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength =6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
