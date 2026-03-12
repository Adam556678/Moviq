using ReservationService.Models;

namespace ReservationService.Services
{
    public interface IShowtimeService
    {
        Task CreateShowtimeAsync(Showtime showtime);

        Task DeleteShowtimeAsync(Guid showtimeId);
    }
}