using TheaterService.Models;

namespace TheaterService.Services
{
    public interface IPricingService
    {
        Task<Dictionary<Guid, decimal>> CalculatePriceAsync(Showtime showtime);
    }
}