namespace ReservationService.Models
{
    public class SeatPricing
    {
        public Guid Id { get; set; }

        public Guid ShowtimeId { get; set; }

        public Guid SeatId { get; set; }

        public decimal Price { get; set; }
    }
}