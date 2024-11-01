using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.HIstory
{
    public class HistoryViewModel
    {
        public int PaymentId { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string ServiceName { get; set; }
        public int AppointmentId { get; set; }
        public string? Description { get; set; }
    }
}