using TheaterService.Enums;

namespace TheaterService.DTOs
{
    public record AddHallDto(
        string Name,
        int TotalSeats,
        HallType HallType = HallType.Standard
    );
}