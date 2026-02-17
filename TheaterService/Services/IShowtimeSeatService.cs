namespace TheaterService.Services
{
    public interface IShowtimeSeatService
    {
        Task InitializeSeatsForShowtime(Guid showtimeId);

        Task HandleSeatLockRequest();

        Task ReserveSeat();
    }
}