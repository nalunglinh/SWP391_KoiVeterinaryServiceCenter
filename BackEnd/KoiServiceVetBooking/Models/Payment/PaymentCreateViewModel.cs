using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Payment
{
    public class PaymentCreateViewModel
    {
    public int CustomerId { get; set; }
    public int DoctorId { get; set; }
    public int AppointmentId { get; set; }
    public int ServiceId { get; set; }
    public string FullName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? UserAddress { get; set; }
    public string? PaymentMethod { get; set; }
    public bool IsHomeVisit { get; set; } = false; // Optional for service ID 3
    }
}