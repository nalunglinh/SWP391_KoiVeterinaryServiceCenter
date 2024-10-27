using Microsoft.EntityFrameworkCore;

namespace KoiServiceVetBooking.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //bảng DB
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DoctorWorkshift> DoctorWorkshift { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Feedback> ServiceFeedbacks { get; set; }
        public DbSet<History> ServiceHistories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Bills> Bills { get; set; }
        public DbSet<DoctorService> DoctorsServices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DoctorWorkshift>().HasKey(dw => dw.WorkshiftId);

            modelBuilder.Entity<DoctorWorkshift>()
                .HasOne(dw => dw.Doctor) // Chỉ định mối quan hệ với Doctor
                .WithMany() // Nếu một bác sĩ có nhiều workshifts
                .HasForeignKey(dw => dw.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); // Hành động khi xóa

            modelBuilder.Entity<DoctorWorkshift>()
                .HasOne(dw => dw.DoctorSchedule) // Chỉ định mối quan hệ với DoctorSchedule
                .WithMany() // Nếu một lịch có nhiều workshifts
                .HasForeignKey(dw => dw.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade); // Hành động khi xóa
        
        }
    }
}
