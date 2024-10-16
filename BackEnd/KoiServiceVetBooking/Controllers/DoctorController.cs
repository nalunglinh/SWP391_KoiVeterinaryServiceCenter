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

        // API để lấy danh sách bác sĩ
        [HttpGet("index")]
        public ActionResult<List<DoctorListViewModel>> Index(string searchTerm)
        {
            // Lấy danh sách bác sĩ từ database
            var doctors = _context.Users
                .Where(u => u.role == "Doctor" && (string.IsNullOrEmpty(searchTerm) || u.FullName.Contains(searchTerm)))
                .Select(s => new DoctorListViewModel
                {
                    DoctorId = s.UserId,
                    FullName = s.FullName,
                    Email = s.Email,
                    UserAddress = s.UserAddress
                })
                .ToList();

            return Ok(doctors);
        }

        // API để book bác sĩ
        [HttpGet("book/{doctorId}")]
        public ActionResult<DoctorListViewModel> Book(int doctorId)
        {
            // Logic xử lý việc booking cho bác sĩ
            var doctor = _context.Users.FirstOrDefault(s => s.UserId == doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            // Trả về thông tin bác sĩ nếu tìm thấy
            var doctorViewModel = new DoctorListViewModel
            {
                DoctorId = doctor.UserId,
                FullName = doctor.FullName,
                Email = doctor.Email,
                UserAddress = doctor.UserAddress
            };

            return Ok(doctorViewModel);
        }
    }
}
