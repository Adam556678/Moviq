using TheaterService.Enums;
using TheaterService.Models;

namespace TheaterService.Data
{
    public interface IPricingRepository
    {
        Task<SeatPricing?> GetSeatPricingAsync(SeatType seatType);
        Task<HallPricing?> GetHallPricingAsync(HallType hallType);
        Task<TimePricing?> GetTimePricingAsync(Showtime showtime);

        // Seat pricing opertaions
        Task AddSeatPricing(SeatPricing pricing);
        Task UpdateSeatPricing(SeatPricing pricing);
        Task RemoveSeatPricing(Guid id);

        // Hall pricing opertaions
        Task AddHallPricing(HallPricing pricing);
        Task UpdateHallPricing(HallPricing pricing);
        Task RemoveHallPricing(Guid id);

        // Time pricing opertaions
        Task AddTimePricing(TimePricing pricing);
        Task UpdateTimePricing(TimePricing pricing);
        Task RemoveTimePricing(Guid id);
    }
}