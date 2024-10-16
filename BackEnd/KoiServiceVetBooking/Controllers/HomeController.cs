using System.Diagnostics;
using KoiServiceVetBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // API cho trang chính (Index)
        [HttpGet("index")]
        public ActionResult<string> Index()
        {
            return Ok("Welcome to the Koi Service Vet Booking API.");
        }

        // API cho trang chính sách quyền riêng tư
        [HttpGet("privacy")]
        public ActionResult<string> Privacy()
        {
            return Ok("This is the privacy policy.");
        }

        // API cho xử lý lỗi
        [HttpGet("error")]
        public ActionResult<ErrorViewModel> Error()
        {
            var errorModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return Problem(detail: errorModel.RequestId);
        }
    }
}
