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
        public virtual required Payment Payment { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual required UserAccount Customer { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public virtual required Service Service { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public virtual required Appointment Appointment { get; set; }
    }
}
