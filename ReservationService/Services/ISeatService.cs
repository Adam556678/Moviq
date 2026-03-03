using ReservationService.Enums;

namespace ReservationService.Services
{
    public interface ISeatService
    {
        Task<bool> IsSeatAvailable(Guid showtimeSeatId);

        Task UpdateSeatStatus(Guid showtimeSeatId, SeatStatus seatStatus);
    }
}