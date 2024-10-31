using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.Payment
{
    public class BillViewModel
    {
        public int BillId { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string ServiceName { get; set; }
        public int AppointmentId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BillDate { get; set; }
        public string BillStatus { get; set; }

    }
}