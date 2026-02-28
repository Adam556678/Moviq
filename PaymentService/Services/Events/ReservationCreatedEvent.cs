namespace PaymentService.Services.Events
{
    public class ReservationCreatedEvent
    {
        public decimal Price { get; set; }
        public string Currency { get; set; } = "usd";
        public int Quantity { get; set; }

        public Guid ReservationId { get; set; }

        public required string MovieName { get; set; }

        public DateTime ShowtimeStart { get; set; }
    }
}