using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class DoctorSchedule
    {
        internal int doctor_id;

        [Key]
        [Column("schedule_id")]
        public int ScheduleId { get; set; }
        [Column("Day_of_week")]
        public required string DayOfWeek { get; set; }
        [Column("Time_from")]
        public TimeSpan TimeFrom { get; set; }
        [Column("Time_to")]
        public TimeSpan TimeTo { get; set; }
    }
}