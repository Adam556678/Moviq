using TheaterService.Enums;

namespace TheaterService.DTOs.PricingDtos
{
    public class AddSeatPricingDto
    {
        public SeatType SeatType { get; set; }

        public decimal BasePrice { get; set; }
    }
}