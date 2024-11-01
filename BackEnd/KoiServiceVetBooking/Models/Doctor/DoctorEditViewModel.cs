using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Doctor
{
    public class DoctorEditViewModel
    {
        public required string FullName { get; set; }
        
        public required string Password { get; set; }

        public required string Email { get; set; }

        public DateTime Dob { get; set; }

        public required string Phone { get; set; }

        public string? UserAddress { get; set; }

        public DateTime ShiftDate { get; set; }
        public bool IsBooked { get; set; }

    }
}