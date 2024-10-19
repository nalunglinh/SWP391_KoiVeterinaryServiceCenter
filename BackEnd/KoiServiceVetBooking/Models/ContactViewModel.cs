using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Your full name is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "5 - 50 characters allowed")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed")]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|email\.com)$", ErrorMessage = "Email must be in a valid format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone number must start with 0 and have 10-11 digits.")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters.")]
        public required string Message { get; set; }

    }
}
