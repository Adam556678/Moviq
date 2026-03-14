namespace ReservationService.Services.Events
{
    public class SeatStatusUpdateResponse
    {
        public ICollection<Guid> SeatIds { get; set; } = new List<Guid>();

        public Guid ReservationId { get; set; }

        public Guid ShowtimeId { get; set; }

        public bool LockSucceeded { get; set; }
    }
}