using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }

        //lấy danh sách dịch vụ (Customer, Admin)
        [HttpGet("List")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public ActionResult<List<ServiceViewModel>> Index()
        {
            var services = _context.Services.Select(s => new ServiceViewModel
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                Description = s.Description
            }).ToList();

            return Ok(services);
        }

        //book dịch vụ (Customer)
        [HttpGet("bookService/{serviceId}")]
        [Authorize(Roles = "Customer,Doctor")]
        public ActionResult<ServiceViewModel> Book(int serviceId)
        {
            // Logic xử lý việc booking cho dịch vụ có ServiceId
            var service = _context.Services.FirstOrDefault(s => s.ServiceId == serviceId);
            if (service == null)
            {
                return NotFound();
            }

            // Trả về thông tin dịch vụ nếu tìm thấy
            var serviceViewModel = new ServiceViewModel
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                Description = service.Description
            };

            return Ok(serviceViewModel);
        }
    }
}
