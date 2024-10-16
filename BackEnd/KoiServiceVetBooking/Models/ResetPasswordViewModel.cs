using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed")]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|email\.com)$", ErrorMessage = "Email must be in a valid format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 20 characters.")]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }


        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
}
