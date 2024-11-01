namespace KoiServiceVetBooking.Models
{
    public class DoctorListViewModel
    {
        public int DoctorId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string UserAddress { get; set; }
    }
}
