using TheaterService.Models;

namespace TheaterService.Services
{
    public interface IShowtimeSeatService
    {
        void InitializeSeatsForShowtime(Showtime showtime);

        Task HandleSeatLockRequest();

        Task HandleSeatReleaseRequest();

        Task ReserveSeat();
    }
}