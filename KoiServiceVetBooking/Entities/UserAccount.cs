using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class UserAccount
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        public required string FullName { get; set; }
        
        public required string Password { get; set; }

        public DateTime Dob { get; set; }

        public required string Phone { get; set; }

        [RegularExpression(@"^[^@\s]+@(gmail\.com|email\.com)$", ErrorMessage = "Email must be in a valid format")]
        public required string Email { get; set; }

        [Column("UserAddress")]
        public string? UserAddress { get; set; }//maybe null

        public required string role { get; set; } // 'Customer', 'Doctor', 'Admin'

        public string Status { get; set; } = "valid"; // Default value: 'valid'
        
    }
}
