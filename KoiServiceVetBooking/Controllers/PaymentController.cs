using KoiServiceVetBooking.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KoiServiceVetBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string bankCode = "BIDV"; // Mã ngân hàng BIDV giả lập
        private readonly string accountNumber = "0073823942"; // Số tài khoản giả lập BIDV

        public PaymentController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // Hàm tạo mã QR giả lập
        private string GenerateMockQRCode(string paymentId, decimal amount)
        {
            // Giả lập URL mã QR bằng thông tin của ngân hàng BIDV và số tài khoản
            var qrCodeUrl = $"https://mock-vietqr.com/generate?bankCode={bankCode}&accountNumber={accountNumber}&amount={(long)amount}&message=ThanhToanDichVu_{paymentId}";
            return qrCodeUrl;
        }

        // Endpoint xử lý khi người dùng nhấn nút "book"
        [HttpPost("vietqr/mock/pay")] // Ràng buộc phương thức HTTP POST
        public async Task<ActionResult> PayWithMockVietQR(int appointmentId, int customerId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.Status == "booked");

            if (appointment == null)
            {
                return NotFound("Không tìm thấy cuộc hẹn hoặc cuộc hẹn chưa được đặt.");
            }

            // Tạo bản ghi thanh toán giả lập
            var payment = new Payment
            {
                CustomerId = customerId,
                AppointmentId = appointment.AppointmentId,
                PaymentMethod = "VietQR_Mock",
                PaymentStatus = "Pending",
                Amount = appointment.Service.Price,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Tạo mã QR giả lập
            string qrCodeUrl = GenerateMockQRCode(payment.PaymentId.ToString(), payment.Amount);

            return Ok(new { QRCodeUrl = qrCodeUrl });
        }

        // Endpoint để mô phỏng thanh toán thành công
        [HttpGet("vietqr/mock/return")] // Ràng buộc phương thức HTTP GET
        public IActionResult MockReturn(string paymentId)
        {
            // Giả lập trạng thái thanh toán thành công
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId.ToString() == paymentId);
            if (payment == null)
            {
                return NotFound("Không tìm thấy giao dịch.");
            }

            // Cập nhật trạng thái thanh toán
            payment.PaymentStatus = "Completed";

            // Tạo hóa đơn sau khi thanh toán thành công
            var bill = new Bills
            {
                CustomerId = payment.CustomerId,
                PaymentId = payment.PaymentId,
                AppointmentId = payment.AppointmentId,
                TotalAmount = payment.Amount,
                BillDate = DateTime.Now,
                BillStatus = "Paid"
            };

            _context.Bills.Add(bill);
            _context.SaveChanges();

            return Ok("Thanh toán giả lập thành công! Hóa đơn đã được tạo.");
        }
    }
}
