using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class History
    {
        [Key]
        [Column("history_id")]
        public int HistoryId { get; set; }

        [Column("payment_id")]
        public int PaymentId { get; set; }
        
        [Column("customer_id")]
        public int CustomerId { get; set; }
        
        [Column("service_id")]
        public int ServiceId { get; set; }
        
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        public UserAccount Customer { get; set; }
        public Service Service { get; set; }
        public Payment Payment { get; set; }
        public Appointment Appointment { get; set; }
    }
}
