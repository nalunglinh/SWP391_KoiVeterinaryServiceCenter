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

        //lấy danh sách lịch sử từ id
        [HttpGet("List/{id}")]
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
        [HttpGet("All-list/{customerId}")]
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

        // Lấy thông tin lịch sử giao dịch của customer
        [HttpGet("Customer/{customerId}")]
        [Authorize(Roles = "Customer,Admin,Doctor")]
        public async Task<ActionResult<IEnumerable<HistoryViewModel>>> GetCustomerHistory(int customerId)
        {
            // Tìm tất cả lịch sử giao dịch cho customer theo customerId
            var historyList = await _context.ServiceHistory
                .Where(h => h.CustomerId == customerId)
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

            if (historyList == null || !historyList.Any())
            {
                return NotFound("No transaction history found for this user.");
            }

            return Ok(historyList);
        }

        //Xóa lịch sử (Admin)
        [HttpDelete("Delete/{historyId}/Customer/{customerId}")]
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