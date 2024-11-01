using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class Feedback
    {
        [Key]
        [Column("feedback_id")]
        public int FeedbackId { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }
        [Column("Comment")]
        public string? Comment { get; set; }
        [Column("Feedback_date")]
        public DateTime? FeedbackDate { get; set;}
    }

}