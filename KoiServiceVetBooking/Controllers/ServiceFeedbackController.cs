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

        // Tạo feedback
        [HttpPost("Submit-Feedback")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackViewModel feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == feedback.CustomerId && u.role == "Customer");
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == feedback.DoctorId && u.role == "Doctor");
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.ServiceId == feedback.ServiceId);

            // Kiểm tra thông tin khách hàng và bác sĩ
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            if (service == null)
            {
                return BadRequest("Service not found.");
            }

            // Tạo đối tượng feedback
            var serviceFeedback = new Feedback
            {
                Comment = feedback.Comment,
                FeedbackDate = DateTime.Now,
                DoctorId = feedback.DoctorId,
                CustomerId = feedback.CustomerId,
                ServiceId = feedback.ServiceId
            };

            await _context.ServiceFeedback.AddAsync(serviceFeedback);
            await _context.SaveChangesAsync();

            // Nếu có RatingValue, thêm rating vào cơ sở dữ liệu
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

                await _context.Rating.AddAsync(rating);
                await _context.SaveChangesAsync();
            }
            return Ok("Feedback submitted successfully.");
        }


        // list feedback
        [HttpGet("GetList")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            return await _context.ServiceFeedback.ToListAsync();
        }

        // tìm feedback theo id
        [HttpGet("FindFeedback/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.ServiceFeedback.FindAsync(id);

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
            var feedback = await _context.ServiceFeedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.ServiceFeedback.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
