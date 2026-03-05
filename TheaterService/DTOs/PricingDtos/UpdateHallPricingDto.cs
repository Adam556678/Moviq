using TheaterService.Enums;

namespace TheaterService.DTOs.PricingDtos
{
    public class UpdateHallPricingDto
    {
        public HallType? HallType { get; set; }

        public decimal? Multiplier { get; set; }
    }
}