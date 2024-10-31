using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Doctor
{
    public class WorkshiftEditViewModel
    {
        public int DoctorId { get; set; }
        public DateTime ShiftDate { get; set; }
        public int? ScheduleId { get; set; }
        public bool? IsBooked { get; set; }
    }
}