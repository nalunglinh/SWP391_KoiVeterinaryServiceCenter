using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Doctor
{
    public class DoctorServiceViewModel
    {
         public int DoctorId { get; set; }
        public List<int> ServiceId { get; set; } 
    }
}