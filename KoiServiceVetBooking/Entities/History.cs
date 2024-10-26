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

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        public UserAccount Customer { get; set; }
        public Service Service { get; set; }
        public Payment Payment { get; set; }
        public Appointment Appointment { get; set; }
    }
}
