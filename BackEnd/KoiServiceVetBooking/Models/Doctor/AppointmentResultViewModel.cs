using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Doctor
{
    public class AppointmentResultViewModel
    {
        public int AppointmentId { get; set; }
        public string? Result { get; set; }
        public string? Description { get; set; }
    }
}