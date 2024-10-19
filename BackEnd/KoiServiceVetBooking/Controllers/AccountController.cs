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

        // Đăng ký tài khoản mới
        [HttpPost("register")]
        public async Task<ActionResult<string>> Registration(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu hay chưa
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
                Status = "valid" // Tài khoản mặc định là hợp lệ
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
        [HttpPost("login")]
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
        [HttpPost("logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out successfully.");
        }

        // Quên mật khẩu
        [HttpPost("forgot-password")]
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

            // Chuyển đến trang reset password nếu ok (Chỉ trả về thông điệp cho API)
            return Ok("Please check your email to reset your password.");
        }

        // Reset mật khẩu
        [HttpPost("reset-password")]
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

        // Lấy thông tin user
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<CustomerProfileViewModel>> Profile()
        {
            // Kiểm tra đăng nhập
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Lấy email
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("Email not found.");
            }

            // Lấy thông tin bằng email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Chuyển thông tin vào ViewModel
            var customerProfileViewModel = new CustomerProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                UserAddress = user.UserAddress ?? "No address provided",
                Dob = user.Dob
            };

            return Ok(customerProfileViewModel);
        }

        // Cập nhật user
        [HttpPut("edit-profile")]
        [Authorize]
        public async Task<ActionResult<string>> EditProfile(CustomerProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found.");
            }

            var userId = int.Parse(userIdClaim);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Dob = model.Dob;
            user.Phone = model.Phone;
            user.UserAddress = model.UserAddress;

            await _context.SaveChangesAsync();
            return Ok("Profile updated successfully.");
        }
    }
}
