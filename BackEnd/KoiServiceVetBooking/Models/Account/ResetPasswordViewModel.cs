using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class ResetPasswordViewModel
    {
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }


        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
}
