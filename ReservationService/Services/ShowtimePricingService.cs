using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.Models;

namespace ReservationService.Services
{
    public class ShowtimePricingService : IShowtimePricingService
    {

        private readonly AppDbContext _context;

        public ShowtimePricingService(AppDbContext context)
        {
            _context = context;
        }


        public async Task CreateShowtimePricing(
            ShowtimePricing pricing,
            Dictionary<Guid, decimal> seatPrices
        )
        {
            var newSeatPrices = seatPrices.Select(kvp => new SeatPricing
            {
               ShowtimeId = pricing.ShowtimeId,
               SeatId = kvp.Key,
               Price = kvp.Value 
            });

            await _context.SeatPricings.AddRangeAsync(newSeatPrices);
            await _context.ShowtimePricings.AddAsync(pricing);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateTotalPrice(
            ICollection<Guid> seatIds, 
            Guid showtimeId
        )
        {
            var seatPricings = await _context.SeatPricings.Where(s => 
                s.ShowtimeId == showtimeId && seatIds.Contains(s.SeatId))
                .ToListAsync();
            
            if (!seatPricings.Any())
                throw new Exception("Pricing for the seats does not exist");
            
            return seatPricings.Sum(s => s.Price);
        }

        public async Task<ShowtimePricing> GetByIdAsync(Guid showtimeId)
        {
            var pricing = await _context.ShowtimePricings
                .Where(p => p.ShowtimeId == showtimeId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            if (pricing == null)
                throw new Exception("Pricing for this showtime does not exist");
            
            return pricing;
        }
    }
}