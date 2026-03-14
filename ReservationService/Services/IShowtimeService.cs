using ReservationService.Models;

namespace ReservationService.Services
{
    public interface IShowtimeService
    {
        Task CreateShowtimeAsync(Showtime showtime);

        Task<Showtime?> GetByIdAsync(Guid id);

        Task DeleteShowtimeAsync(Guid showtimeId);
    }
}