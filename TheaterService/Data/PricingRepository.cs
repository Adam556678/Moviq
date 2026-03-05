using Microsoft.EntityFrameworkCore;
using TheaterService.Enums;
using TheaterService.Models;

namespace TheaterService.Data
{
    public class PricingRepository : IPricingRepository
    {

        private readonly AppDbContext _context;

        public PricingRepository(AppDbContext context)
        {
            _context = context;
        }

        // ---------- Getters ----------
        public async Task<HallPricing?> GetHallPricingAsync(HallType hallType)
        {
            var pricing = await _context.HallPricing.FirstOrDefaultAsync(
                p => p.HallType == hallType);
            
            if (pricing == null)
                throw new Exception("Pricing does not exist");

            return pricing;
        }

        public async Task<SeatPricing?> GetSeatPricingAsync(SeatType seatType)
        {
            var pricing = await _context.SeatPricing.FirstOrDefaultAsync(
                p => p.SeatType == seatType
            );

            if (pricing == null)
                throw new Exception("Pricing does not exist");

            return pricing;
        }

        public async Task<TimePricing?> GetTimePricingAsync(Showtime showtime)
        {
            // Convert startTime to hours
            var showtimeTime = showtime.StartTime.TimeOfDay;

            var rules = await _context.TimePricing.ToListAsync();
            
            // return time pricing of this start time
            return rules.FirstOrDefault(r => 
            (r.StartHour <= r.EndHour &&
                r.StartHour <= showtimeTime && 
                r.EndHour > showtimeTime)
            ||
            (r.StartHour > r.EndHour &&
                (r.StartHour <= showtimeTime || 
                r.EndHour > showtimeTime))
            );
        }

        // ---------- HallPricing ----------
        public Task AddHallPricing(HallPricing pricing)
        {
            throw new NotImplementedException();
        }

        public Task UpdateHallPricing(HallPricing pricing)
        {
            throw new NotImplementedException();
        }

        public Task RemoveHallPricing(Guid id)
        {
            throw new NotImplementedException();
        }

        // ---------- SeatPricing ----------
        public Task AddSeatPricing(SeatPricing pricing)
        {
            throw new NotImplementedException();
        }

        public Task RemoveSeatPricing(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSeatPricing(SeatPricing pricing)
        {
            throw new NotImplementedException();
        }

        // ---------- TimePricing ----------
        public Task AddTimePricing(TimePricing pricing)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTimePricing(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTimePricing(TimePricing pricing)
        {
            throw new NotImplementedException();
        }
    }
}