using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Appoinment
{
    public class AppointmentConsultViewModel
    {
        public int AppointmentId { get; set;}
        public int CustomerId { get; set; }
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Description { get; set; }
    }
}