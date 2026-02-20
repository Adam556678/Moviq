using TheaterService.Enums;

namespace TheaterService.DTOs
{
    public record UpdateSeatDto(
        int? Row,
        int? Column,
        SeatType? SeatType,
        bool? IsFunctional
    );
}