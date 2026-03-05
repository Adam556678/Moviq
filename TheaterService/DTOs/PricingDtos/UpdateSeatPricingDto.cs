using TheaterService.Enums;

namespace TheaterService.DTOs.PricingDtos
{
    public class UpdateSeatPricingDto
    {
        public SeatType? SeatType { get; set; }

        public decimal? BasePrice { get; set; }
    }
}