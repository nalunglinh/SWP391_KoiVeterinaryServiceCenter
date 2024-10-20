using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class CustomerProfileViewModel
    {
        public int UserId { get; set; }

        public required string FullName { get; set; }
        
        public required string Email { get; set; }

        public DateTime Dob { get; set; }

        public required string Phone { get; set; }

        public string? UserAddress { get; set; }
    }
}
