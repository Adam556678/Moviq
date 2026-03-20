namespace ReservationService.Services.Events
{
    public class ReservationCreatedEvent
    {
        public decimal TotalPrice { get; set; } 
        public string Currency { get; set; } = "usd";

        public Guid ReservationId { get; set; }

        public required string MovieName { get; set; }
        public required string HallName { get; set; }

        public List<Guid> SeatIds { get; set; } = new List<Guid>();
        public DateTime ShowtimeStart { get; set; }
    }
}