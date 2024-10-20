using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using KoiServiceVetBooking.Models;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public ActionResult SendContactForm(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Gửi email
            _emailService.SendContactEmail(model);

            return Ok("Your message has been sent successfully.");
        }
    }

}
