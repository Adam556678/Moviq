namespace TheaterService.Services.Events
{
    public class ShowtimePricingPublishedEvent
    {
        public Guid ShowtimeId { get; set; }

        public required Dictionary<Guid, decimal> SeatPrices { get; set; }
    }
}