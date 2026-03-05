using Microsoft.EntityFrameworkCore;
using TheaterService.DTOs.PricingDtos;
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
        public async Task AddHallPricing(HallPricing pricing)
        {
            var hallPricing = await _context.HallPricing
                .FirstOrDefaultAsync(hp => hp.HallType == pricing.HallType);
            
            if (hallPricing != null)
                throw new Exception("Pricing with this halltype already exists");
            
            await _context.HallPricing.AddAsync(pricing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHallPricing(Guid id, UpdateHallPricingDto pricingDto)
        {
            var existingHallPricing = await _context.HallPricing.
                FirstOrDefaultAsync(hp => hp.Id == id);
            if (existingHallPricing == null)
                throw new Exception("HallPricing with this ID doesn't exist");

            if (pricingDto.HallType != null)
                existingHallPricing.HallType = pricingDto.HallType.Value;
            if (pricingDto.Multiplier != null)
                existingHallPricing.Multiplier = pricingDto.Multiplier.Value;

            _context.HallPricing.Update(existingHallPricing);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveHallPricing(Guid id)
        {
            var hallPricing = await _context.HallPricing.FindAsync(id);
            if (hallPricing == null)
                throw new Exception("HallPricing with this ID doesn't exist");

            _context.HallPricing.Remove(hallPricing);
            await _context.SaveChangesAsync();
        }

        // ---------- SeatPricing ----------
        public async Task AddSeatPricing(SeatPricing pricing)
        {
            var seatPricing = await _context.SeatPricing
                .FirstOrDefaultAsync(sp => sp.SeatType == pricing.SeatType);
            if (seatPricing != null)
                throw new Exception("SeatPricing with this seat type already exists");

            await _context.SeatPricing.AddAsync(pricing);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSeatPricing(Guid id)
        {
            var seatPricing = await _context.SeatPricing.FindAsync(id);
            if (seatPricing == null)
                throw new Exception("SeatPricing with this id doesn't exist");

            _context.SeatPricing.Remove(seatPricing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSeatPricing(Guid id, UpdateSeatPricingDto seatPricingDto)
        {
            var seatPricing = await _context.SeatPricing.FindAsync(id);
            if (seatPricing == null)
                throw new Exception("SeatPricing with this id doesn't exist");

            if (seatPricingDto.SeatType != null)
                seatPricing.SeatType = seatPricingDto.SeatType.Value;
            if (seatPricingDto.BasePrice != null)
                seatPricing.BasePrice = seatPricingDto.BasePrice.Value;

            _context.SeatPricing.Update(seatPricing);
            await _context.SaveChangesAsync();
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

        public Task UpdateTimePricing(Guid id, UpdateTimePricingDto timePricingDto)
        {
            throw new NotImplementedException();
        }
    }
}