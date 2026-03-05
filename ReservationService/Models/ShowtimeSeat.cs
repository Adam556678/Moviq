using ReservationService.Enums;

namespace ReservationService.Models
{
    public class ShowtimeSeat
    {
        public Guid Id { get; set; }

        public required Guid SeatId { get; set; }

        public Guid ShowtimeId { get; set; }

        public virtual Showtime Showtime { get; set; } = default!;

        public SeatStatus Status { get; set; } = SeatStatus.Available;

    }
}