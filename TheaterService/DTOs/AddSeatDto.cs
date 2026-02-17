using TheaterService.Enums;

namespace TheaterService.DTOs
{
    public record AddSeatDto(
        int Row,
        int Column,
        Guid HallId, 
        SeatType SeatType = SeatType.Standard
    );
}