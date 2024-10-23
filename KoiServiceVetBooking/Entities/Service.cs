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

        public double Price { get; set; }

        public double Surcharge { get; set; }
    }
}
