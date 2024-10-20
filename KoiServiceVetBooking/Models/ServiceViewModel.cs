using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiServiceVetBooking.Models
{
    public class ServiceViewModel
    {
        public int ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
    }
}