using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Entities
{
    public class DoctorService
    {
        [Key]
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }

        [ForeignKey("DoctorId")]
        public UserAccount Doctor { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
    }
}