using TheaterService.Enums;

namespace TheaterService.Models
{
    public class SeatPricing
    {
        public Guid Id { get; set; }

        public SeatType SeatType { get; set; }

        public decimal BasePrice { get; set; }
    }
}