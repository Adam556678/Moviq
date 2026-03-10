namespace TheaterService.Services.Events
{
    public class ShowtimeCreatedEvent
    {
        public Guid ShowtimeId { get; set; }

        public required string MovieTitle { get; set; }

        public required string HallName { get; set; }
    }
}