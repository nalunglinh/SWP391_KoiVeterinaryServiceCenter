using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models.HIstory
{
    public class HistoryCreateViewModel
    {
        public int PaymentId { get; set; }
        public int ServiceId { get; set; }
        public int AppointmentId { get; set; }
    }
}