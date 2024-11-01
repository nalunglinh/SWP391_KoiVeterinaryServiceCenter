using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class LoginViewModel
    {
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

    }
}
