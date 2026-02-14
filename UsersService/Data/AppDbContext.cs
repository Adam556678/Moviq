using Microsoft.EntityFrameworkCore;
using UsersService.Models;

namespace UsersService.Data
{
public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserCredentials)
                .WithOne(c => c.User)
                .HasForeignKey<UserCredentials>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<UserOTP> UserOTPs { get; set; }
    }   
}