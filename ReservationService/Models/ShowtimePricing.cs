namespace ReservationService.Models
{
    public class ShowtimePricing
    {
        public Guid Id { get; set; }
        public Guid ShowtimeId { get; set; }
        // public required Dictionary<Guid, decimal> SeatPrices { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}