using TheaterService.Models;

namespace TheaterService.Services
{
    public interface IShowtimeSeatService
    {
        void InitializeSeatsForShowtime(Showtime showtime);

        Task<bool> TryLockSeatAsync(Guid showtimeId, List<Guid> seatIds);

        Task<bool> TakeSeatAsync(Guid showtimeId, List<Guid> seatIds);

    }
}