using TheaterService.Enums;
using TheaterService.Models;

namespace TheaterService.Data
{
    public class PricingRepository : IPricingRepository
    {
        // ---------- Getters ----------
        public Task<HallPricing?> GetHallPricingAsync(HallType hallType)
        {
            throw new NotImplementedException();
        }

        public Task<SeatPricing?> GetSeatPricingAsync(SeatType seatType)
        {
            throw new NotImplementedException();
        }

        public Task<TimePricing?> GetTimePricingAsync(Showtime showtime)
        {
            throw new NotImplementedException();
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