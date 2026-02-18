namespace TheaterService.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
    }
}