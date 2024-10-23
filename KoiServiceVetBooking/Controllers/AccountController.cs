using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // Đăng ký tài khoản mới (chỉ có thể là customer - admin và doctor được cấp tài khoản sẵn)
        [HttpPost("Register")]
        public async Task<ActionResult<string>> Registration(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem email đã tồn tại trong CSDL hay chưa
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered. Please use a different email.");
                return BadRequest(ModelState);
            }

            UserAccount account = new UserAccount
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone,
                role = "Customer", // role mặc định là Customer
                Status = "valid" //tkhoan mặc định khi đki là còn sử dụng (valid)
            };

            try
            {
                await _context.Users.AddAsync(account);
                await _context.SaveChangesAsync();
                return Ok($"{account.FullName} registered successfully. Please log in.");
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError("", $"Database error: {errorMessage}");
                return BadRequest(ModelState);
            }
        }

        // Đăng nhập
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                .Where(x => x.Email.ToLower() == model.Email.ToLower() && x.Password == model.Password)
                .FirstOrDefaultAsync();
                
            if (user == null)
            {
                return Unauthorized("Email or Password is not correct.");
            }

            var userAddress = user.UserAddress ?? "Not updated yet";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim("Name", user.FullName),
                new Claim(ClaimTypes.Role, user.role ?? "Customer"),
                new Claim("UserAddress", userAddress)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Đăng nhập với cookie
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return Ok("Login successful.");
        }

        // Đăng xuất
        [HttpPost("Logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out successfully.");
        }

        // Quên mật khẩu
        [HttpPost("Forgot-password")]
        public async Task<ActionResult<string>> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra email tồn tại
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound("Email does not exist.");
            }

            // Chuyển đến trang reset password nếu ok
            return Ok("Please check your email to reset your password.");
        }

        // Reset mật khẩu
        [HttpPost("Reset-password")]
        public async Task<ActionResult<string>> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound("Email does not exist.");
            }

            user.Password = model.NewPassword;
            await _context.SaveChangesAsync();

            return Ok("Password reset successfully.");
        }

    }
}
