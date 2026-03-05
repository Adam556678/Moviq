using Microsoft.EntityFrameworkCore;
using TheaterService.Models;

namespace TheaterService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<ShowtimeSeat> ShowtimeSeats { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Movie> Movies { get; set; }

        // Pricings
        public DbSet<SeatPricing> SeatPricing { get; set; }
        public DbSet<HallPricing> HallPricing { get; set; }
        public DbSet<TimePricing> TimePricing { get; set; }
    }
}