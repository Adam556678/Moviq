namespace ReservationService.Models
{
    public class Showtime
    {
        public Guid Id { get; set; }

        public string MovieName { get; set; }

        public DateTime StartTime { get; set; }
    }
}