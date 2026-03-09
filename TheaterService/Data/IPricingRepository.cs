using TheaterService.DTOs.PricingDtos;
using TheaterService.Enums;
using TheaterService.Models;

namespace TheaterService.Data
{
    public interface IPricingRepository
    {
        Task<SeatPricing?> GetSeatPricingAsync(SeatType seatType);
        Task<IEnumerable<SeatPricing>> GetAllSeatPricingsAsync();
        Task<HallPricing?> GetHallPricingAsync(HallType hallType);
        Task<TimePricing?> GetTimePricingAsync(Showtime showtime);

        // Seat pricing opertaions
        Task AddSeatPricing(SeatPricing pricing);
        Task UpdateSeatPricing(Guid id, UpdateSeatPricingDto seatPricingDto);
        Task RemoveSeatPricing(Guid id);

        // Hall pricing opertaions
        Task AddHallPricing(HallPricing pricing);
        Task UpdateHallPricing(Guid id, UpdateHallPricingDto pricingDto);
        Task RemoveHallPricing(Guid id);

        // Time pricing opertaions
        Task AddTimePricing(TimePricing pricing);
        Task UpdateTimePricing(Guid id, UpdateTimePricingDto timePricingDto);
        Task RemoveTimePricing(Guid id);
    }
}