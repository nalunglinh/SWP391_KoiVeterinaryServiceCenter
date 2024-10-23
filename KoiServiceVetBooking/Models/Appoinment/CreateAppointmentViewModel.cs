using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Appointment
{
    public class CreateAppointmentViewModel
    {
        public int CustomerId { get; set; }

        public int DoctorId { get; set; }

        public int ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }

        public string? Place { get; set; }

        public string? Description { get; set; }
    }

}