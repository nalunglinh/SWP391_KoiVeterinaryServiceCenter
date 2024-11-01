using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using KoiServiceVetBooking.Models.Appointment;
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
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        //Create lịch hẹn 
        [HttpPost("create-appointment")]
        public async Task<ActionResult> CreateAppointment(CreateAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra bác sĩ
            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.DoctorId && u.role == "Doctor");
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            // Kiểm tra dịch vụ
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == model.ServiceId);
            if (service == null)
            {
                return NotFound("Service not found.");
            }

            // Tạo lịch hẹn
            var appointment = new Appointment
            {
                CustomerId = model.CustomerId,
                DoctorId = model.DoctorId,
                ServiceId = model.ServiceId,
                AppointmentDate = model.AppointmentDate,
                Place = model.Place ?? "No address provided",
                Status = "pending" // Mặc định trạng thái là 'pending'
            };

            // Lưu vào database
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment created successfully.", appointmentId = appointment.AppointmentId });

        }

        //lấy list lịch hẹn
        [HttpGet("List-appointment")]
        public async Task<ActionResult<List<Appointment>>> GetAppointments(int? appointmentId)
        {

            var query = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Doctor)
                .Include(a => a.Service);


            if (appointmentId.HasValue)
            {
                var appointment = await query
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
                if (appointment == null)
                {
                    return NotFound();
                }
                return Ok(new List<Appointment> { appointment }); // Trả về danh sách với một phần tử
            }

            // Nếu không có appointmentId, lấy tất cả các cuộc hẹn
            var appointments = await query.ToListAsync();
            return Ok(appointments);
        }


        //lấy chi tiết lịch hẹn
        [HttpGet("Detail/{appointmentId}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        //Update trạng thái đặt hẹn
        [HttpPut("Update/status/{appointmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateStatusAppointment(int appointmentId, Appointment appointment)
        {
            if (appointmentId != appointment.AppointmentId)
            {
                return BadRequest("ID lịch hẹn không khớp.");
            }

            if (appointment.Status == "Pending")
            {
                appointment.Status = "Paid"; // Cập nhật trạng thái
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointmentId))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        //delete lịch hẹn
        [HttpDelete("Delete/{appointmentId}")]
        public async Task<ActionResult> DeleteAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Update status lịch hẹn
        [HttpPatch("status/{appointmentId}")]
        public async Task<ActionResult> UpdateAppointmentStatus(int appointmentId, string status)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //lấy lịch sử hẹn
        [HttpGet("history/{customerId}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentHistory(int customerId)
        {
            var history = await _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();

            return Ok(history);
        }

        //gửi feedback lịch hẹn về hệ thống
        [HttpPost("feedback/{appointmentId}")]
        public async Task<ActionResult> SendFeedback(int appointmentId, string feedback)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Feedback = feedback;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int appointmentId)
        {
            return _context.Appointments.Any(e => e.AppointmentId == appointmentId);
        }
    }
}
