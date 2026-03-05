using TheaterService.Enums;

namespace TheaterService.Models
{
    public class HallPricing
    {
        public Guid Id { get; set; }

        public HallType HallType { get; set; }

        public decimal Multiplier { get; set; }
    }
}