using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using Microsoft.AspNetCore.Mvc;
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

        // Tìm bác sĩ theo ID
        [HttpGet("doctor/id/{doctorId}")]
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
        [HttpGet("doctor/{doctorId}")]
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
                    ShiftDate = dw.ShiftDate
                }).FirstOrDefault();

                if (doctorProfile == null)
                {
                    return NotFound();
                }
            return Ok(doctorProfile);
        }

        //List lịch làm việc available
        [HttpGet("available-workshifts/{doctorId}")]
        public ActionResult<List<DoctorWorkshift>> GetAvailableWorkshifts(int doctorId)
        {
            var workshifts = _context.DoctorWorkshift
                .Where(dw => dw.DoctorId == doctorId && !dw.IsBooked) // Lọc lịch chưa được đặt
                .ToList();

            return Ok(workshifts);
        }

        //book lịch hẹn với bác sĩ
        [HttpPost("book/{doctorId}")]
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
                return BadRequest("Khung giờ đéo khả dụng");
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
