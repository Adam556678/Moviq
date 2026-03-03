namespace ReservationService.Models
{
    public class ReservationSeat
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Guid ShowtimeSeatId { get; set; }

        public Reservation Reservation { get; set; } = default!;
    }
}