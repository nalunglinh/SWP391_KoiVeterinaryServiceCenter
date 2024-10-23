using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class RegisterViewModel
    {
        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
}
