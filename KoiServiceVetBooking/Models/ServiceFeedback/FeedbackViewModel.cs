using System.ComponentModel.DataAnnotations;

namespace KoiServiceVetBooking.Models
{
    public class FeedbackViewModel
    {
        public required int customerId { get; set; }
        public required string CustomerName { get; set; }
        public int DoctorId { get; set; }
        public required string DoctorName { get; set; }
        public int ServiceId { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
        
    }
}
