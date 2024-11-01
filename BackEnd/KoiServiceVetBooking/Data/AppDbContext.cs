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
        public DbSet<Feedback> ServiceFeedback { get; set; }
        public DbSet<History> ServiceHistory { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Bills> Bills { get; set; }
        public DbSet<DoctorService> DoctorsServices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DoctorWorkshift>().HasKey(dw => dw.WorkshiftId);

            modelBuilder.Entity<DoctorWorkshift>()
                .HasOne(dw => dw.Doctor) // Chỉ định mối quan hệ với Doctor
                .WithMany()
                .HasForeignKey(dw => dw.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DoctorWorkshift>()
                .HasOne(dw => dw.DoctorSchedule) // Chỉ định mối quan hệ với DoctorSchedule
                .WithMany()
                .HasForeignKey(dw => dw.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<DoctorService>()
                .HasOne(ds => ds.Doctor)
                .WithMany()
                .HasForeignKey(ds => ds.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); 
        
        }
    }
}
