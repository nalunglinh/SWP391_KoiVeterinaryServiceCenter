using KoiServiceVetBooking.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KoiServiceVetBooking.Models.Payment;
using KoiServiceVetBooking.Models.HIstory;
using Microsoft.AspNetCore.Authorization;

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

        //tạo payment và lưu vào history
        [HttpPost("create")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreatePayment(PaymentCreateViewModel model)
        {
            var service = await _context.Services.FindAsync(model.ServiceId);
            if (service == null)
            {
                return BadRequest("Service not found!");
            }

            // Calculate surcharge based on service type and visit preference
            decimal surcharge = 0;
            if (model.ServiceId == 2)
            {
                surcharge = service.Surcharge;
            }
            else if (model.ServiceId == 3 && model.IsHomeVisit)
            {
                surcharge = service.Surcharge;
            }

            // Calculate the total amount
            var amount = service.Price + surcharge;

            var payment = new Payment
            {
                CustomerId = model.CustomerId,
                AppointmentId = model.AppointmentId,
                PaymentMethod = model.PaymentMethod,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = "Pending"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var serviceHistory = new History
            {
                PaymentId = payment.PaymentId,
                CustomerId = model.CustomerId,
                ServiceId = model.ServiceId,
                AppointmentId = model.AppointmentId
            };


            _context.ServiceHistory.Add(serviceHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            return Ok(payment);
        }

        // Hàm tạo mã QR giả lập
        private string GenerateMockQRCode(string paymentId, decimal amount)
        {
            var qrCodeUrl = $"https://img.vietqr.io/image/BIDV-39148219491-compact.png";
            return qrCodeUrl;
        }

        // Endpoint xử lý khi người dùng nhấn nút "book"
        [HttpPost("vietqr/mock/pay")]
        public async Task<ActionResult> PayWithMockVietQR(int appointmentId, int customerId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return NotFound("Appointment not found or appointment not yet booked!");
            }

            // Tạo bản ghi thanh toán giả lập
            var payment = new Payment
            {
                CustomerId = customerId,
                AppointmentId = appointment.AppointmentId,
                PaymentMethod = "VietQR",
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
        [HttpGet("vietqr/mock/return")]
        public IActionResult MockReturn(string paymentId)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId.ToString() == paymentId);
            if (payment == null)
            {
                return NotFound("No transactions found!");
            }

            // Cập nhật trạng thái thanh toán
            payment.PaymentStatus = "Paid";

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

            return Ok("Payment successful! Invoice has been created.");
        }

        // Kiểm tra trạng thái thanh toán
        [HttpGet("status/{paymentId}")]
        public async Task<IActionResult> CheckPaymentStatus(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            return Ok(new { PaymentId = payment.PaymentId, PaymentStatus = payment.PaymentStatus });
        }
    }
}
