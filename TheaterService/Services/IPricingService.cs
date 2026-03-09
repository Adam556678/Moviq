using TheaterService.Models;

namespace TheaterService.Services
{
    public interface IPricingService
    {
        Task<decimal> CalculatePriceAsync(Showtime showtime);
    }
}