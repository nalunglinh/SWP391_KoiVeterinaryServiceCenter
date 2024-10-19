using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class UserAccount
    {
        [Key]
        [Column("user_id")] // ánh xạ tới tên cột trong bảng
        public int UserId { get; set; }
        [Column("FullName")]
        public required string FullName { get; set; }
        [Column("Password")]
        public required string Password { get; set; }
        [Column("Dob")]
        public DateTime Dob { get; set; }
        [Column("Phone")]
        public required string Phone { get; set; }
        [Column("Email")]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|email\.com)$", ErrorMessage = "Email must be in a valid format")]
        public required string Email { get; set; }
        [Column("UserAddress")]
        public string? UserAddress { get; set; }//maybe null
        [Column("role")]
        public required string role { get; set; } // 'Customer', 'Doctor', 'Admin'
        [Column("Status")]
        public string Status { get; set; } = "valid"; // Default value: 'valid'
    }
}
