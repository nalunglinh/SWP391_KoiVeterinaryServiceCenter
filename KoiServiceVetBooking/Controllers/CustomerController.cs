using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // Lấy thông tin profile của Customer
        [HttpGet("profile")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<CustomerProfileViewModel>> Profile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail && u.role == "Customer");
            if (user == null)
            {
                return NotFound("Customer profile not found.");
            }

            var customerProfile = new CustomerProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                UserAddress = user.UserAddress ?? "No address provided",
                Dob = user.Dob
            };

            return Ok(customerProfile);
        }

        // Chỉnh sửa profile của Customer
        [HttpPut("edit-profile")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<string>> EditCustomerProfile(CustomerProfileViewModel model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail && u.role == "Customer");

            if (user == null)
            {
                return NotFound("Customer not found.");
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Dob = model.Dob;
            user.Phone = model.Phone;
            user.UserAddress = model.UserAddress;

            await _context.SaveChangesAsync();
            return Ok("Profile updated successfully.");
        }

        // tìm customer by ID
        [HttpGet("Profile/id/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomerProfileViewModel>> GetCustomerById(int id)
        {
            var customer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id && u.role == "Customer");
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var customerProfile = new CustomerProfileViewModel
            {
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                UserAddress = customer.UserAddress,
                Dob = customer.Dob
            };

            return Ok(customerProfile);
        }

        // Chỉnh sửa profile của Customer (admin)
        [HttpPut("Profile/Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCustomer(int id, CustomerProfileViewModel model)
        {
            var customer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id && u.role == "Customer");
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            customer.FullName = model.FullName;
            customer.Email = model.Email;
            customer.Phone = model.Phone;
            customer.UserAddress = model.UserAddress;

            await _context.SaveChangesAsync();
            return Ok("Customer updated successfully.");
        }

        // xóa profile của Customer (admin)
        [HttpDelete("Profile/Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id && u.role == "Customer");
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            _context.Users.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok("Customer deleted successfully.");
        }

    }
}