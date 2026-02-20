using TheaterService.Enums;

namespace TheaterService.DTOs
{
    public record AddHallDto(
        string Name,
        int NumRows,
        int NumColumns,
        bool AutoSeatInit, // Automatically initializes seats grid 
        HallType HallType = HallType.Standard
    );
}