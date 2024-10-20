using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiServiceVetBooking.Entities
{
    public class Appointment
    {
        [Key]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [ForeignKey("customer_id")]
        public required int CustomerId { get; set; }

        [ForeignKey("doctor_id")]
        public required int DoctorId { get; set; }

        [ForeignKey("service_id")]
        public required int ServiceId { get; set; }

        [Column("Appointment_date")]
        public DateTime AppointmentDate { get; set; }

        public required string Place { get; set; }

        public string Status { get; set; } = "pending"; // Default value

        public string? Result { get; set; }

        public string? Description { get; set; }

        public string? Feedback { get; set; }

        // public required UserAccount Customer_id { get; set; }
        // public required UserAccount Doctor_id { get; set; }
        // public required Service service_id { get; set; }
    }
        
}