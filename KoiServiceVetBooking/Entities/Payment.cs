using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public int PaymentId { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("appointment_id")]
        public int AppointmentId { get; set; }
        [Column("Payment_method")]
        public string? PaymentMethod { get; set; }
        [Column("Payment_status")]
        public string PaymentStatus { get; set; } = "Pending";
        [Column("Amount")]
        public decimal Amount { get; set; }
        [Column("Payment_date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        [Column("IsHomeVisit")]
        public bool IsHomeVisit { get; set; } = false;
    }
}
