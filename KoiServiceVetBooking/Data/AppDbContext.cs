using Microsoft.EntityFrameworkCore;

namespace KoiServiceVetBooking.Entities
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) 
        {

        }
        //bảng DB
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DoctorWorkshift> DoctorWorkshift { get; set; }
        public DbSet<Appointment> Appointments{ get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Feedback> ServiceFeedbacks { get; set; }
        public DbSet<History> ServiceHistories { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
