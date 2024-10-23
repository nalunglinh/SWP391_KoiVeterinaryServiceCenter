using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }

        // Thay thế user_id bằng customer_id
        public int CustomerId { get; set; }

        public int AppointmentId { get; set; }

        [Column("Payment_method")]
        public string? PaymentMethod { get; set; }
        [Column("Payment_status")]
        public string PaymentStatus { get; set; } = "Pending"; // Giá trị mặc định
        [Column("Amount")]
        public decimal Amount { get; set; }
        [Column("Payment_date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now; // Giá trị mặc định
    }
}
