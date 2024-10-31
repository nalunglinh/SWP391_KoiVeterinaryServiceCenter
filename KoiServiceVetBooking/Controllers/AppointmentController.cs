using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using KoiServiceVetBooking.Models.Appoinment;
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

        //lấy list lịch hẹn (Admin, Doctor)
        [HttpGet("List-appointment")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<List<CreateAppointmentViewModel>>> GetAppointments(int? appointmentId)
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

                var appointmentViewModel = new CreateAppointmentViewModel
                {
                    AppointmentId = appointment.AppointmentId,
                    CustomerId = appointment.CustomerId,
                    DoctorId = appointment.DoctorId,
                    ServiceId = appointment.ServiceId,
                    AppointmentDate = appointment.AppointmentDate,
                    Place = appointment.Place,
                    Description = appointment.Description
                };

                return Ok(new List<CreateAppointmentViewModel> { appointmentViewModel });
            }

            // Nếu không có -> lấy tất cả
            var appointments = await query.ToListAsync();
            var appointmentViewModels = appointments.Select(appointment => new CreateAppointmentViewModel
            {
                AppointmentId = appointment.AppointmentId,
                CustomerId = appointment.CustomerId,
                DoctorId = appointment.DoctorId,
                ServiceId = appointment.ServiceId,
                AppointmentDate = appointment.AppointmentDate,
                Place = appointment.Place,
                Description = appointment.Description
            }).ToList();

            return Ok(appointmentViewModels);
        }

        //lấy chi tiết lịch hẹn (Admin, Doctor)
        [HttpGet("Detail/{appointmentId}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // //Update trạng thái đặt hẹn (Admin)
        // [HttpPut("Update/status/{appointmentId}")]
        // [Authorize(Roles = "Admin")]
        // public async Task<ActionResult> UpdateStatusAppointment(int appointmentId, Appointment appointment)
        // {
        //     if (appointmentId != appointment.AppointmentId)
        //     {
        //         return BadRequest("ID lịch hẹn không khớp.");
        //     }

        //     if (appointment.Status == "Pending")
        //     {
        //         appointment.Status = "Paid";
        //     }

        //     _context.Entry(appointment).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!AppointmentExists(appointmentId))
        //         {
        //             return NotFound();
        //         }
        //         throw;
        //     }

        //     return NoContent();
        // }

        //delete lịch hẹn (Admin)
        [HttpDelete("Delete/{appointmentId}")]
        [Authorize(Roles = "Admin")]
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

        //Update status lịch hẹn (Admin, Doctor)
        [HttpPatch("status/{appointmentId}")]
        [Authorize(Roles = "Admin,Doctor")]
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

        //với service = 1, đặt lịch hẹn không cần giờ cụ thể (Customer)
        [HttpPost("Consult")] 
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateAppointmentNoTime([FromBody] AppointmentConsultViewModel model)
        {
            var service = await _context.Services.FindAsync(model.ServiceId);
            if (service == null)
            {
                return BadRequest("Service does not exist.");
            }

            if (model.ServiceId == 1)
            {
                var appointment = new Appointment
                {
                    CustomerId = model.CustomerId,
                    DoctorId = model.DoctorId,
                    ServiceId = model.ServiceId,
                    AppointmentDate = model.AppointmentDate.Date,
                    Description = model.Description,
                    Status = "pending"
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok("Appointment created successfully without specific time or workshift.");
            }
            else
            {
                return BadRequest("This service does not support appointments without specific time.");
            }
        }

        // Hủy lịch hẹn
        [HttpDelete("Cancel/{appointmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CancelAppointment(int appointmentId)
        {
            // Tìm lịch hẹn
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Cập nhật trạng thái lịch hẹn thành "cancel"
            appointment.Status = "cancel";

            // Tìm payment
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);
            if (payment == null)
            {
                return BadRequest("No payment record found for this appointment.");
            }

            // Lưu vào lịch sử (ServiceHistory)
            var serviceHistory = new History
            {
                PaymentId = payment.PaymentId,
                CustomerId = appointment.CustomerId,
                ServiceId = appointment.ServiceId,
                AppointmentId = appointment.AppointmentId,
            };
            await _context.ServiceHistory.AddAsync(serviceHistory);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment canceled successfully." });
        }

        //gửi feedback lịch hẹn về hệ thống (Customer)
        [HttpPost("feedback/{appointmentId}")]
        [Authorize(Roles = "Customer")]
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

        //lấy lịch sử hẹn (Customer, Admin)
        [HttpGet("history/{customerId}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentHistory(int customerId)
        {
            var history = await _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();

            return Ok(history);
        }

        private bool AppointmentExists(int appointmentId)
        {
            return _context.Appointments.Any(e => e.AppointmentId == appointmentId);
        }


    }
}
