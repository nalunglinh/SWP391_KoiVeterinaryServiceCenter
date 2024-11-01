using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class Appointment
    {
        [Key]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [Column("customer_id")]
        public required int CustomerId { get; set; }

        [Column("doctor_id")]
        public required int DoctorId { get; set; }

        [Column("service_id")]
        public required int ServiceId { get; set; }

        [Column("Appointment_date")]
        public DateTime AppointmentDate { get; set; }

        public required string Place { get; set; }

        public string Status { get; set; } = "pending"; // Default value

        public string? Result { get; set; }

        public string? Description { get; set; }

        public string? Feedback { get; set; }

        // Các mối quan hệ
        [ForeignKey("CustomerId")]
        public virtual UserAccount Customer { get; set; }

        [ForeignKey("DoctorId")]
        public virtual UserAccount Doctor { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}
