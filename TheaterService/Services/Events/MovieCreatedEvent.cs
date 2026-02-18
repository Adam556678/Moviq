namespace TheaterService.Services.Events
{
    public class MovieCreatedEvent
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
    }
}