using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
   public class DoctorWorkshift
    {
        [Key]
        [Column("workshift_id")]
        public int WorkshiftId { get; set; }

        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("Shift_date")]
        public DateTime ShiftDate { get; set; }
        
        public bool IsBooked { get; set; } = false;

        // Khai báo mối quan hệ với DoctorSchedule
        public virtual DoctorSchedule DoctorSchedule { get; set; }

        // Khai báo mối quan hệ với UserAccount (Doctor)
        public virtual UserAccount Doctor { get; set; }
    }

}