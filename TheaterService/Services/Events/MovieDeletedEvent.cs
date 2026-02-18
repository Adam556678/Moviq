namespace TheaterService.Services.Events
{
    public class MovieDeletedEvent
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
    }
}