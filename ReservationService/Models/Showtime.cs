namespace ReservationService.Models
{
    public class Showtime
    {
        public Guid Id { get; set; }

        public Guid ShowtimeId { get; set; }

        public required string MovieName { get; set; }
        public required string HallName { get; set; }

        public DateTime StartTime { get; set; }
    }
}