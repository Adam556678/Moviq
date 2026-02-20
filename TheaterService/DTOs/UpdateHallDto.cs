using TheaterService.Enums;

namespace TheaterService.DTOs
{
    public record UpdateHallDto(
        string? Name,
        int? NumRows,
        int? NumColumns,
        HallType? HallType
    );
}