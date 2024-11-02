using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Doctor
{
    public class DoctorBookingViewModel
    {
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Place { get; set; }
        public string? Description { get; set; }
        public bool IsHomeVisit { get; set; } = false;
        
    }
}