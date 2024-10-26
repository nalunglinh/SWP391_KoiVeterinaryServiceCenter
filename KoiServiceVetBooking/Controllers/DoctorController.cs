using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
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

            // Lấy ID của bác sĩ mới tạo
            int doctorId = doctor.UserId;

            // Danh sách các ngày trong tuần mà bác sĩ làm việc
            var workDays1 = new List<string> { "Monday", "Wednesday", "Friday" };
            var workDays2 = new List<string> { "Tuesday", "Thursday", "Saturday" };

            DateTime startDate = DateTime.Today;

            // Thêm ca làm việc vào bảng DoctorWorkshift cho từng ngày trong danh sách workDays1 và workDays2
            foreach (var day in workDays1.Concat(workDays2))
            {
                // Lấy lịch trình làm việc từ DoctorSchedules dựa trên DayOfWeek
                var schedule = await _context.DoctorSchedules
                    .FirstOrDefaultAsync(s => s.DayOfWeek == day);

                if (schedule != null)
                {
                    // Tạo ca làm việc cho từng ngày
                    var workshift = new DoctorWorkshift
                    {
                        DoctorId = doctorId,
                        ScheduleId = schedule.ScheduleId,
                        ShiftDate = GetNextDateForDay(startDate, day),
                        IsBooked = false,
                        DoctorSchedule = schedule,
                        Doctor = doctor
                    };

                    _context.DoctorWorkshift.Add(workshift);
                }
            }
            await _context.SaveChangesAsync();

            return Ok("Doctor created successfully with multiple shifts.");
        }

        private DateTime GetNextDateForDay(DateTime startDate, string dayOfWeek)
        {
            DayOfWeek targetDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeek);
            int daysUntilTarget = ((int)targetDay - (int)startDate.DayOfWeek + 7) % 7;
            return startDate.AddDays(daysUntilTarget);
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
                // Khởi tạo ngày làm việc bắt đầu từ ShiftDate
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDoctor(int id, DoctorProfileViewModel model)
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

            await _context.SaveChangesAsync();
            return Ok("Doctor updated successfully.");
        }

        // xóa Doctor
        [HttpDelete("Profile/Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id && u.role == "Doctor");
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

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

        //thông tin bác sĩ và thời gian làm việc
        [HttpGet("Doctor/Profile/{doctorId}")]
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
        [HttpGet("Doctor/Workshift/{doctorId}")]
        public ActionResult<List<DoctorWorkshift>> GetAvailableWorkshifts(int doctorId)
        {
            var workshifts = _context.DoctorWorkshift
                .Where(dw => dw.DoctorId == doctorId && !dw.IsBooked) // Lọc lịch chưa được đặt
                .ToList();

            return Ok(workshifts);
        }

        //book lịch hẹn với bác sĩ
        [HttpPost("Doctor/Appointment/{doctorId}")]
        public ActionResult<DoctorListViewModel> Book(int doctorId)
        {
            // Tìm bác sĩ
            var doctor = _context.Users.FirstOrDefault(s => s.UserId == doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            var workShift = _context.DoctorWorkshift
                .FirstOrDefault(dw => dw.DoctorId == doctorId && !dw.IsBooked);

            if (workShift == null)
            {
                return BadRequest("the time for work is invalid");
            }

            workShift.IsBooked = true;
            _context.SaveChanges();

            var doctorViewModel = new DoctorListViewModel
            {
                DoctorId = doctor.UserId,
                FullName = doctor.FullName,
                Email = doctor.Email,
                UserAddress = doctor.UserAddress ?? "Unknown doctor address",
            };

            return Ok(doctorViewModel);
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
