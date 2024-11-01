using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class Bills
    {
        [Key]
        [Column("bill_id")]
        public int BillId { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("payment_id")]
        public int PaymentId { get; set; }
        [Column("appointment_id")]
        public int AppointmentId { get; set; }
        [Column("Total_amount")]
        public decimal TotalAmount { get; set; }
        [Column("Bill_date")]
        public DateTime BillDate { get; set; }
        [Column("Bill_status")]
        public string BillStatus { get; set; }
    }
}
