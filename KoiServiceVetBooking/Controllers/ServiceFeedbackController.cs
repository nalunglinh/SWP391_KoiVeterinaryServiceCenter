using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceFeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceFeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // tạo feedback
        [HttpPost("Submit-Feedback")]
        [Authorize(Roles = "Customer")]
        public IActionResult SubmitFeedback([FromBody] FeedbackViewModel feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _context.Users.FirstOrDefault(u => u.UserId == feedback.customerId && u.role == "Customer");
            var doctor = _context.Users.FirstOrDefault(u => u.UserId == feedback.DoctorId && u.role == "Doctor");

            string customerName = customer != null ? customer.FullName : "Unknown Customer";
            string doctorName = doctor != null ? doctor.FullName : "Unknown Doctor";

            var serviceFeedback = new Feedback
            {
                Comment = feedback.Comment,
                FeedbackDate = DateTime.Now,
                DoctorId = feedback.DoctorId,
                ServiceId = feedback.ServiceId,
                CustomerId = feedback.customerId
            };

            _context.ServiceFeedbacks.Add(serviceFeedback);
            _context.SaveChanges();

            var service = _context.Services.FirstOrDefault(s => s.ServiceId == feedback.ServiceId);

            if (service == null)
            {
                return BadRequest("Service not found.");
            }

            if (feedback.RatingValue.HasValue)
            {
                var rating = new Rating
                {
                    AppointmentId = serviceFeedback.FeedbackId,
                    DoctorId = feedback.DoctorId,
                    ServiceId = feedback.ServiceId,
                    RatingValue = feedback.RatingValue.Value,
                    Service = service
                };

                _context.Rating.Add(rating);
                _context.SaveChanges();
            }

            // Trả về phản hồi thành công
            return Ok("Feedback submitted successfully.");
        }

        // list feedback
        [HttpGet("GetList")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            return await _context.ServiceFeedbacks.ToListAsync();
        }

        // tìm feedback theo id
        [HttpGet("FindFeedback/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.ServiceFeedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        // Xóa feedback
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.ServiceFeedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.ServiceFeedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
