using KoiServiceVetBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        // GET: api/contact
        [HttpGet]
        public ActionResult<string> Index()
        {
            return "Contact form. Please send a POST request to submit your message.";
        }

        // POST: api/contact
        [HttpPost]
        public ActionResult<string> Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Xử lý thông tin liên hệ (lưu vào database, gửi email, v.v.)
                // Gửi email thông báo hoặc lưu vào database

                return Ok("Thank you for contacting us. We will get back to you soon.");
            }

            return BadRequest("Invalid data. Please check your input.");
        }
    }
}
