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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
