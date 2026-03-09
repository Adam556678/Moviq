using TheaterService.Data;
using TheaterService.Models;

namespace TheaterService.Services
{
    public class PricingService : IPricingService
    {

        private IPricingRepository _pricingRepository;

        public PricingService(IPricingRepository pricingRepository)
        {
            _pricingRepository = pricingRepository;
        }

        public async Task<decimal> CalculatePriceAsync(Showtime showtime)
        {
            // get hall pricing
            var hallPricing = await _pricingRepository.GetHallPricingAsync(showtime.Hall.HallType);
            if (hallPricing == null)
                throw new Exception("Hall pricing for this hall type does not exist");

            // get time pricing
            var timePricing = await _pricingRepository.GetTimePricingAsync(showtime);
            if (timePricing == null)
                throw new Exception("Time pricing for this showtime does not exist");

            // get seat pricings
            var allSeatPricings = await _pricingRepository.GetAllSeatPricingsAsync();
            decimal totalBasePrice = 0;

            foreach (var showtimeSeat in showtime.SeatStates)
            {
                var pricing = allSeatPricings.FirstOrDefault(p => p.SeatType == showtimeSeat.Seat.SeatType);
                if (pricing == null)
                    throw new Exception($"pricing for seat type: {showtimeSeat.Seat.SeatType} deos not exist");
                
                totalBasePrice += pricing.BasePrice;
            }

            return totalBasePrice * hallPricing.Multiplier * timePricing.Multiplier;
        }
    }
}