namespace TheaterService.DTOs
{
    public record UpdateShowtimeDto(
        Guid? MovieId,
        DateTime? StartTime,
        Guid? HallId
    );
}