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
        public async Task<IActionResult> CreatePayment(int appointmentId, [FromBody] PaymentCreateViewModel model)
        {
            // Tìm lịch hẹn dựa trên AppointmentId
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            
            if (appointment == null)
            {
                return BadRequest("Appointment not found!");
            }

            // Lấy thông tin dịch vụ từ lịch hẹn
            var service = await _context.Services.FindAsync(appointment.ServiceId);
            if (service == null)
            {
                return BadRequest("Service not found!");
            }

            // Tính toán phụ phí thăm khám tại nhà
            decimal surcharge = 0;
            if (appointment.ServiceId == 2)
            {
                surcharge = service.Surcharge;
            }
            else if (appointment.ServiceId == 3 && model.IsHomeVisit)
            {
                surcharge = service.Surcharge;
            }
            var amount = service.Price + surcharge;

            // Tạo mới bản ghi thanh toán
            var payment = new Payment
            {
                CustomerId = appointment.CustomerId,
                AppointmentId = appointmentId,
                PaymentMethod = model.PaymentMethod,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = "Pending"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Tạo bản ghi trong lịch sử dịch vụ
            var serviceHistory = new History
            {
                PaymentId = payment.PaymentId,
                CustomerId = appointment.CustomerId,
                ServiceId = appointment.ServiceId,
                AppointmentId = appointmentId
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
        public async Task<ActionResult> PayWithMockVietQR(int customerId)
        {
            // Find the appointment by customer ID (assuming unique appointments per customer)
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.CustomerId == customerId);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Check for existing payment to prevent duplicates
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.AppointmentId == appointment.AppointmentId);
            if (existingPayment != null)
            {
                return BadRequest("Payment has already been made for this appointment.");
            }

            // Create a new payment record
            var payment = new Payment
            {
                CustomerId = customerId,
                AppointmentId = appointment.AppointmentId,
                PaymentMethod = "VietQR",
                PaymentStatus = "Paid",
                Amount = appointment.Service.Price,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Generate mock QR code URL
            string qrCodeUrl = GenerateMockQRCode(payment.PaymentId.ToString(), payment.Amount);

            // Return the generated QR code URL
            return Ok(new { QRCodeUrl = qrCodeUrl });
        }
        


        //thanh toán thành công
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

        //tạo Bill
        [HttpPost("Bill/{paymentId}")]
        // [Authorize(Roles = "Customer")]
        public async Task<ActionResult<BillViewModel>> Bill(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null || payment.PaymentStatus != "Paid")
            {
                return BadRequest("Payment not found or not completed.");
            }

            var appointment = await _context.Appointments.FindAsync(payment.AppointmentId);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Lấy thông tin của dịch vụ
            var service = await _context.Services.FindAsync(appointment.ServiceId);
            if (service == null)
            {
                return NotFound("Service not found.");
            }

            var bill = new Bills
            {
                CustomerId = payment.CustomerId,
                PaymentId = payment.PaymentId,
                AppointmentId = appointment.AppointmentId,
                TotalAmount = payment.Amount,
                BillDate = DateTime.Now,
                BillStatus = "Paid"
            };

            await _context.Bills.AddAsync(bill);
            await _context.SaveChangesAsync();

            // Tạo đối tượng View Model để trả về
            var billViewModel = new BillViewModel
            {
                BillId = bill.BillId,
                CustomerId = bill.CustomerId,
                AppointmentId = bill.AppointmentId,
                TotalAmount = bill.TotalAmount,
                BillDate = bill.BillDate,
                BillStatus = bill.BillStatus,
                FullName = (await _context.Users.FindAsync(payment.CustomerId)).FullName,
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                Price = service.Price,
                Surcharge = service.Surcharge
            };

            return Ok(billViewModel);
        }

    }
}
