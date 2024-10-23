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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateDoctor(DoctorEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            return Ok("Doctor created successfully.");
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

    }
}
