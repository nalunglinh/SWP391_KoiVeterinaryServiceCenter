using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Your full name is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "5 - 50 characters allowed")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed")]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|email\.com)$", ErrorMessage = "Email must be in a valid format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone number must start with 0 and have 10-11 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters.")]
        public string Message { get; set; }

    }
}
