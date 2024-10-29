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

        // tạo Doctor
        [HttpPost("Create-doctor")]
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

        //tạo workshift cho doctor
       [HttpPost("Create-Workshift")]
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

        // Tìm bác sĩ theo ID
        [HttpGet("Profile/id/{doctorId}")]
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

         // chỉnh sửa Doctor
        [HttpPut("Profile/Update/{id}")]
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

        // xóa Doctor
        [HttpDelete("Profile/Delete/{id}")]
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

        //list doctor
        [HttpGet("List-of-Doctor")]
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

        // Get all doctors
        [HttpGet("All-of-Doctors")]
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

        // Get doctors by service
        [HttpGet("List-by-Service/{serviceId}")]
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

        //thông tin bác sĩ và thời gian làm việc
        [HttpGet("Profile/{doctorId}")]
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

        //List lịch làm việc available
        [HttpGet("Workshift/{doctorId}")]
        public ActionResult<List<DoctorWorkshift>> GetAvailableWorkshifts(int doctorId)
        {
            var workshifts = _context.DoctorWorkshift
                .Where(dw => dw.DoctorId == doctorId && !dw.IsBooked) // Lọc lịch chưa được đặt
                .ToList();

            return Ok(workshifts);
        }

        //book lịch hẹn với bác sĩ
        [HttpPost("Booking/{doctorId}")]
        public async Task<ActionResult<DoctorBookingViewModel>> Book(int doctorId, int workshiftId, DoctorBookingViewModel model)
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
                Status = "pending" // Mặc định trạng thái là 'pending'
            };

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment created successfully.", appointmentId = appointment.AppointmentId });
        }

        [HttpPut("Appointment/Result")]
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
