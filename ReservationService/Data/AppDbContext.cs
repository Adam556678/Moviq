using Microsoft.EntityFrameworkCore;
using ReservationService.Models;

namespace ReservationService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationSeat> ReservationSeats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<ShowtimePricing> ShowtimePricings { get; set; }
        public DbSet<SeatPricing> SeatPricings { get; set; }
    }

}