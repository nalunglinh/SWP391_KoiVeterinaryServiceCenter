using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using KoiServiceVetBooking.Models.Appointment;
using KoiServiceVetBooking.Models.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }

        // tạo Doctor (Admin)
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateDoctor(DoctorCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.ServiceId < 1 || model.ServiceId > 3)
            {
                return BadRequest("ServiceId must be 1, 2, or 3.");
            }

            // Tạo tài khoản bác sĩ
            var doctor = new UserAccount
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone,
                UserAddress = model.UserAddress,
                role = "Doctor",
                Status = "valid"
            };

            _context.Users.Add(doctor);
            await _context.SaveChangesAsync();

            //chỉ định doctor và dịch vụ 
            var doctorService = new DoctorService
            {
                DoctorId = doctor.UserId,
                ServiceId = model.ServiceId
            };

            _context.DoctorsServices.Add(doctorService);
            await _context.SaveChangesAsync();

            return Ok("Doctor created successfully");
        }

        // Tìm bác sĩ theo ID (Admin)
        [HttpGet("Find/{doctorId}")]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult<DoctorListViewModel> GetDoctorById(int doctorId)
        {
            var doctor = _context.Users
                .Where(u => u.UserId == doctorId && u.role == "Doctor")
                .Select(s => new DoctorListViewModel
                {
                    DoctorId = s.UserId,
                    FullName = s.FullName,
                    Email = s.Email,
                    UserAddress = s.UserAddress ?? "Unknown doctor address",
                })
                .FirstOrDefault();

            // Kiểm tra có tồn tại
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

         // chỉnh sửa Doctor (Admin)
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDoctor(int id, int workshiftId, DoctorEditViewModel model)
        {
            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id && u.role == "Doctor");
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            doctor.FullName = model.FullName;
            doctor.Email = model.Email;
            doctor.Phone = model.Phone;
            doctor.UserAddress = model.UserAddress;

            // Tìm lịch làm việc
            var workshift = await _context.DoctorWorkshift.FirstOrDefaultAsync(ws => ws.WorkshiftId == workshiftId && ws.DoctorId == id);
            if (workshift != null)
            {
                // Cập nhật thông tin lịch làm việc
                workshift.ShiftDate = model.ShiftDate;
                workshift.IsBooked = model.IsBooked;
            }
            else
            {
                return NotFound("Workshift not found.");
            }

            await _context.SaveChangesAsync();
            return Ok("Doctor updated successfully.");
        }

        // xóa Doctor (Admin)
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id && u.role == "Doctor");
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            // Tìm và xóa các workshift liên quan
            var workshifts = _context.DoctorWorkshift.Where(ws => ws.DoctorId == id);
            _context.DoctorWorkshift.RemoveRange(workshifts);

            // Xóa các bản ghi liên quan trong DoctorsServices trước khi xóa Doctor
            var doctorServices = _context.DoctorsServices.Where(ds => ds.DoctorId == id);
            _context.DoctorsServices.RemoveRange(doctorServices);

            _context.Users.Remove(doctor);
            await _context.SaveChangesAsync();
            return Ok("Doctor deleted successfully.");
        }

        //Search doctor (Customer, Admin)
        [HttpGet("Search-Doctor")]
        [Authorize(Roles = "Customer,Admin")]
        public ActionResult<List<DoctorListViewModel>> ListDoctor(string searchTerm)
        {
            // Lấy list doctor từ db
            var doctors = _context.Users
                .Where(u => u.role == "Doctor" && (string.IsNullOrEmpty(searchTerm) || u.FullName.Contains(searchTerm)))
                .Select(s => new DoctorListViewModel
                {
                    DoctorId = s.UserId,
                    FullName = s.FullName,
                    Email = s.Email,
                    UserAddress = s.UserAddress ?? "Unknown doctor address",
                })
                .ToList();

            return Ok(doctors);
        }

        // Get all doctors (Admin)
        [HttpGet("All-Doctors")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<DoctorListViewModel>>> ListAllDoctors()
        {
            var doctors = await _context.Users
                .Where(u => u.role == "Doctor")
                .Select(s => new DoctorListViewModel
                {
                    DoctorId = s.UserId,
                    FullName = s.FullName,
                    Email = s.Email,
                    UserAddress = s.UserAddress ?? "Unknown doctor address",
                })
                .ToListAsync();

            return Ok(doctors);
        }

        // Phân công dịch vụ cho bác sĩ
        [HttpPost("Service/{doctorId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignServicesToDoctor(DoctorServiceViewModel model)
        {
            // Kiểm tra bác sĩ có tồn tại không
            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.DoctorId && u.role == "Doctor");
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            // Tạo danh sách bác sĩ
            var doctorServices = model.ServiceId.Select(serviceId => new DoctorService
            {
                DoctorId = model.DoctorId,
                ServiceId = serviceId
            });

            await _context.DoctorsServices.AddRangeAsync(doctorServices);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Services assigned to doctor successfully.", 
            doctorId = model.DoctorId, assignedServices = model.ServiceId });
        }


        // Get doctors bằng service
        [HttpGet("List-by-Service/{serviceId}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public async Task<ActionResult<List<DoctorListViewModel>>> ListDoctorsByService(int serviceId)
        {
            var doctors = await _context.DoctorsServices
                .Where(ds => ds.ServiceId == serviceId)
                .Select(ds => new DoctorListViewModel
                {
                    DoctorId = ds.Doctor.UserId,
                    FullName = ds.Doctor.FullName,
                    Email = ds.Doctor.Email,
                    UserAddress = ds.Doctor.UserAddress ?? "Unknown doctor address",
                })
                .ToListAsync();

            return Ok(doctors);
        }

        //thông tin bác sĩ và thời gian làm việc (Customer, Doctor)
        [HttpGet("Profile/{doctorId}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public ActionResult<DoctorProfileViewModel> GetDoctorProfile(int doctorId, DateTime ShiftDate)
        {
            var doctor = _context.Users.FirstOrDefault(u => u.UserId == doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            // Lấy thông tin lịch làm việc của doctor
            var workShifts = _context.DoctorWorkshift
                .Where(ws => ws.DoctorId == doctorId && ws.IsBooked == false)
                .Join(_context.DoctorSchedules,
                    ws => ws.ScheduleId,
                    ds => ds.ScheduleId,
                    (ws, ds) => new 
                    {
                        ws.ShiftDate,
                        ds.TimeFrom,
                        ds.TimeTo
                    })
                .ToList();

            var doctorProfile = ( from ds in _context.DoctorSchedules
                                join dw in _context.DoctorWorkshift on ds.ScheduleId equals dw.ScheduleId
                                join u in _context.Users on dw.DoctorId equals u.UserId
                                where dw.DoctorId == doctorId && dw.ShiftDate == ShiftDate
                                select new DoctorProfileViewModel
                {
                    DoctorId = doctorId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    UserAddress = u.UserAddress ?? "Unknown doctor address",
                    DayOfWeek = ds.DayOfWeek,
                    TimeFrom = ds.TimeFrom,
                    TimeTo = ds.TimeTo,
                    ShiftDate = dw.ShiftDate,
                    IsBooked = dw.IsBooked 
                }).FirstOrDefault();

                if (doctorProfile == null)
                {
                    return NotFound();
                }
            return Ok(doctorProfile);
        }

        //tạo workshift cho doctor (Admin)
        [HttpPost("Create-Workshift")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateDoctorWorkshift([FromBody] WorkshiftCreateViewModel model)
        {
            var doctorExists = await _context.Users.AnyAsync(u => u.UserId == model.DoctorId && u.role == "Doctor");
            var scheduleExists = await _context.DoctorSchedules.AnyAsync(s => s.ScheduleId == model.ScheduleId);

            if (!doctorExists || !scheduleExists)
            {
                return BadRequest("Doctor or schedule does not exist.");
            }

            // Lặp qua các ngày trong tuần được chỉ định
            foreach (var day in model.DaysOfWeek)
            {
                DateTime shiftDate = model.ShiftDate;

                // Cập nhật ngày làm việc dựa trên ngày trong tuần
                while (shiftDate.DayOfWeek.ToString() != day)
                {
                    shiftDate = shiftDate.AddDays(1);
                }

                // Tạo workshift mới với các thuộc tính bắt buộc
                var doctorWorkshift = new DoctorWorkshift
                {
                    DoctorId = model.DoctorId,
                    ScheduleId = model.ScheduleId,
                    ShiftDate = shiftDate,
                    IsBooked = false
                };

                // Thêm vào ngữ cảnh
                _context.DoctorWorkshift.Add(doctorWorkshift);
            }

            await _context.SaveChangesAsync();
            return Ok("Workshifts created successfully for the doctor.");
        }

        //chỉnh sửa workshift (Admin)
        [HttpPut("Edit-Workshift")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditDoctorWorkshift([FromBody] WorkshiftEditViewModel model)
        {
            var doctorExists = await _context.Users.AnyAsync(u => u.UserId == model.DoctorId && u.role == "Doctor");
            var workshift = await _context.DoctorWorkshift
                .FirstOrDefaultAsync(ws => ws.DoctorId == model.DoctorId && ws.ShiftDate == model.ShiftDate);

            if (!doctorExists || workshift == null)
            {
                return BadRequest("Doctor or workshift does not exist.");
            }

            // Cập nhật các thuộc tính của workshift nếu có
            workshift.ScheduleId = model.ScheduleId ?? workshift.ScheduleId;
            workshift.IsBooked = model.IsBooked ?? workshift.IsBooked;

            _context.DoctorWorkshift.Update(workshift);
            await _context.SaveChangesAsync();

            return Ok("Workshift updated successfully.");
        }

        //xóa workshift (Admin)
        [HttpDelete("Delete-Workshift/{doctorId}/{workshiftId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDoctorWorkshift(int doctorId, int workshiftId)
        {
            // Tìm workshift dựa trên doctorId và workshiftId
            var workshift = await _context.DoctorWorkshift
                .FirstOrDefaultAsync(ws => ws.DoctorId == doctorId && ws.WorkshiftId == workshiftId);

            if (workshift == null)
            {
                return NotFound("Workshift does not exist.");
            }

            _context.DoctorWorkshift.Remove(workshift);
            await _context.SaveChangesAsync();

            return Ok("Workshift deleted successfully.");
        }

        //List lịch làm việc available (Admin, Doctor)
        [HttpGet("List-Workshift/{doctorId}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public ActionResult<List<DoctorWorkshift>> GetAvailableWorkshifts(int doctorId)
        {
            var workshifts = _context.DoctorWorkshift
                .Where(dw => dw.DoctorId == doctorId && !dw.IsBooked) // Lọc lịch chưa được đặt
                .ToList();

            return Ok(workshifts);
        }

        //book lịch hẹn với bác sĩ (Customer) cho service 2 và 3 
        [HttpPost("Booking/Service-2-3/{doctorId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<DoctorBookingViewModel>> Book_Service23(int doctorId, int workshiftId, DoctorBookingViewModel model)
        {
            // Tìm bác sĩ
            var doctor = _context.Users.FirstOrDefault(s => s.UserId == doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

             var workShift = _context.DoctorWorkshift.FirstOrDefault(ws => 
                ws.DoctorId == doctorId && ws.WorkshiftId == workshiftId && !ws.IsBooked);

            if (workShift == null)
            {
                return BadRequest("The specified work shift is not available.");
            }

            workShift.IsBooked = true;
            _context.SaveChanges();

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
                Description = model.Description,
                Status = "success",
                IsHomeVisit = model.IsHomeVisit,
            };

            //đánh dấu cho isBooked của workshift
            workShift.IsBooked = true;
            _context.DoctorWorkshift.Update(workShift);

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment created successfully.", appointmentId = appointment.AppointmentId });
        }

        //book lịch hẹn với bác sĩ (Customer) cho service 1
        [HttpPost("Booking/service-1/{doctorId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<DoctorBookingService1ViewModel>> Book_Service1(int doctorId, int workshiftId, DoctorBookingViewModel model)
        {
            // Tìm bác sĩ
            var doctor = _context.Users.FirstOrDefault(s => s.UserId == doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

             var workShift = _context.DoctorWorkshift.FirstOrDefault(ws => 
                ws.DoctorId == doctorId && ws.WorkshiftId == workshiftId && !ws.IsBooked);

            if (workShift == null)
            {
                return BadRequest("The specified work shift is not available.");
            }

            workShift.IsBooked = true;
            _context.SaveChanges();

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
                Description = model.Description,
                Status = "success",
            };

            //đánh dấu cho isBooked của workshift
            workShift.IsBooked = true;
            _context.DoctorWorkshift.Update(workShift);

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment created successfully.", appointmentId = appointment.AppointmentId });
        }

        //cập nhật lại kết quả sau khi thăm khám (Doctor)
        [HttpPut("Appointment/Result")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult> UpdateAppointmentResult([FromBody] AppointmentResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm appointment
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == model.AppointmentId);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Cập nhật kết quả
            appointment.Result = model.Result;
            appointment.Description = model.Description;

            await _context.SaveChangesAsync();
            return Ok("Appointment result updated successfully.");
        }
    }
}
