using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Doctor
{
    public class WorkshiftCreateViewModel
    {
    public int DoctorId { get; set; }
    public int ScheduleId { get; set; }
    public DateTime ShiftDate { get; set; }
    public List<string> DaysOfWeek { get; set; }
    }

}