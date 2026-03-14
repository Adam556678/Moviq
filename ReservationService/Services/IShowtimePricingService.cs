using ReservationService.Models;

namespace ReservationService.Services
{
    public interface IShowtimePricingService
    {
        Task CreateShowtimePricing(
            ShowtimePricing pricing,
            Dictionary<Guid, decimal> seatPrices
        );

        Task<ShowtimePricing> GetByIdAsync(Guid showtimeId);

        Task<decimal> CalculateTotalPrice(ICollection<Guid> seatIds, Guid showtimeId);
    }
}