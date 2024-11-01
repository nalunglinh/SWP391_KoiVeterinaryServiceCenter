using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class ForgotPasswordViewModel
    {
        public required string Email { get; set; }
    }
}
