using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class Rating
    {
        [Key]
        [Column("rating_id")]
        public int RatingId { get; set;}
        [Column("Rating_value")]
        public int RatingValue { get; set;}
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set;}
        [ForeignKey("Doctor")]
        public int DoctorId { get; set;}
        [ForeignKey("Service")]
        public int ServiceId { get; set;}
        public required Service Service { get; set; }

    }
}