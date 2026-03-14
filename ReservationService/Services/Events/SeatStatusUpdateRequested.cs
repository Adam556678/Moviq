namespace ReservationService.Services.Events
{
    public class SeatStatusUpdateRequested
    {
        public Guid ShowtimeId { get; set; }

        public ICollection<Guid> SeatIds { get; set; } = new List<Guid>();

        public StatusRequest StatusRequest { get; set; }
    }

    public enum StatusRequest
    {
        Lock,
        Unlock
    }
}