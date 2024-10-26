using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Entities
{
    public class Service
    {
        [Key]
        [Column("service_id")]
        public int ServiceId { get; set; }

        [Column("Service_name")]
        public required string ServiceName { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Surcharge { get; set; }

         public ICollection<Appointment> Appointments { get; set; }
    }
}
