namespace TheaterService.DTOs
{
    public record AddShowtimeDto(
        Guid MovieId,
        Guid HallId,
        DateTime StartTime
    );
}