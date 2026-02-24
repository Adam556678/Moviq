using TheaterService.Enums;

namespace TheaterService.DTOs
{
    public class ShowtimeSeatDto
    {
        public Guid Id { get; set; }
        public Guid SeatId { get; set; }
        public Guid ShowtimeId { get; set; }
        public SeatState Status { get; set; }

        public DateTime? LockExpiration { get; set; }
    }
}