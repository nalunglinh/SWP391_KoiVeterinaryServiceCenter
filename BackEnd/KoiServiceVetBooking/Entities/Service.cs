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

        [Column("Description")]
        public required string Description { get; set; }

        [Column("Price")]
        public double Price { get; set; }

        [Column("Surcharge")]
        public double Surcharge { get; set; }
    }
}
