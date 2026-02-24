namespace TheaterService.DTOs
{
    public class ShowtimeReadDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public Guid HallId { get; set; }
        public List<ShowtimeSeatDto> SeatStates { get; set; } = new();
    }
}