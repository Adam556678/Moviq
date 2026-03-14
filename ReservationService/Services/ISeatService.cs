using ReservationService.Enums;

namespace ReservationService.Services
{
    public interface ISeatService
    {
        Task<bool> IsSeatExists(Guid showtimeSeatId);

        Task UpdateSeatStatus(Guid showtimeSeatId, SeatStatus seatStatus);
    }
}