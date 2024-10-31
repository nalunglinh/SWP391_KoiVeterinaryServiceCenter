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
            // Lấy danh sách dịch vụ từ database và chuyển thành ServiceViewModel
            var services = _context.Services.Select(s => new ServiceViewModel
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                Description = s.Description
            }).ToList();

            return Ok(services);
        }

    }
}
