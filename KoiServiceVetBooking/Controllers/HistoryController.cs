using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiServiceVetBooking.Entities;
using KoiServiceVetBooking.Models.HIstory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoryController(AppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }

        //lấy danh sách lịch sử bởi id
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public async Task<ActionResult<History>> GetServiceHistoryById(int id)
        {
            var serviceHistory = await _context.ServiceHistory.FindAsync(id);
            if (serviceHistory == null)
            {
                return NotFound();
            }

            return Ok(serviceHistory);
        }

        //Lấy danh sách tất cả các lịch sử giao dịch (Customer, Admin)
        [HttpGet("Customer/{customerId}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<HistoryViewModel>> GetHistoryByCustomerId(int customerId)
        {
            // Tìm lịch sử giao dịch cho customer
            var history = await _context.ServiceHistory.Where(h => h.CustomerId == customerId)
                .Select(h => new HistoryViewModel
                {
                    PaymentId = h.PaymentId,
                    PaymentMethod = h.Payment.PaymentMethod,
                    Amount = h.Payment.Amount,
                    FullName = h.Customer.FullName,
                    Email = h.Customer.Email,
                    ServiceName = h.Service.ServiceName,
                    AppointmentId = h.AppointmentId,
                    Description = h.Appointment.Description
                })
                .ToListAsync();

            if (history == null || !history.Any())
            {
                return NotFound("No transaction history found for this user");
            }

            return Ok(history);
        }

        //Lấy thông tin chi tiết lịch sử giao dịch cụ thể dựa trên id
        [HttpGet("{historyId}/Customer/{customerId}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public async Task<ActionResult<HistoryViewModel>> GetHistoryDetails(int historyId, int customerId)
        {
            // Tìm chi tiết lịch sử giao dịch cho customer theo historyId
            var history = await _context.ServiceHistory
                .Where(h => h.HistoryId == historyId && h.CustomerId == customerId)
                .Select(h => new HistoryViewModel
                {
                    PaymentId = h.PaymentId,
                    PaymentMethod = h.Payment.PaymentMethod,
                    Amount = h.Payment.Amount,
                    FullName = h.Customer.FullName,
                    Email = h.Customer.Email,
                    ServiceName = h.Service.ServiceName,
                    AppointmentId = h.AppointmentId,
                    Description = h.Appointment.Description
                })
                .FirstOrDefaultAsync();

            if (history == null)
            {
                return NotFound("No transaction history found for this user");
            }

            return Ok(history);
        }

        //Tạo lịch sử giao dịch mới (Customer)
        [HttpPost("CreateHistory/{customerId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateHistory(int customerId, HistoryCreateViewModel model)
        {
            // Kiểm tra xem customer có tồn tại hay không
            var customer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == customerId && u.role == "Customer");
            if (customer == null)
            {
                return NotFound("No customers found");
            }

            // Kiểm tra xem thanh toán có tồn tại và hợp lệ không
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == model.PaymentId && p.CustomerId == customerId);
            if (payment == null)
            {
                return NotFound("Payment information not found");
            }

            // Tạo lịch sử giao dịch mới
            var history = new History
            {
                PaymentId = model.PaymentId,
                CustomerId = customerId,
                ServiceId = model.ServiceId,
                AppointmentId = model.AppointmentId
            };

            _context.ServiceHistory.Add(history);
            await _context.SaveChangesAsync();

            return Ok("Transaction history created successfully");
        }

        //Xóa lịch sử (Admin)
        [HttpDelete("DeleteHistory/{historyId}/Customer/{customerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteHistory(int historyId, int customerId)
        {
            var history = await _context.ServiceHistory
                .FirstOrDefaultAsync(h => h.HistoryId == historyId && h.CustomerId == customerId);

            if (history == null)
            {
                return NotFound("No transaction history found for this customer");
            }

            _context.ServiceHistory.Remove(history);
            await _context.SaveChangesAsync();

            return Ok("Transaction history has been successfully deleted");
        }

    }
}