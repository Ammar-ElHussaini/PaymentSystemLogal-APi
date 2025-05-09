using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 digits.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must contain only digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }
    }
}
