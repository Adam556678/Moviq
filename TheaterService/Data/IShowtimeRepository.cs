using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Data
{
    public interface IShowtimeRepository
    {
        Task AddShowtimeAsync(Showtime showtime);

        Task<IEnumerable<Showtime>> GetAllShowtimesAsync();

        Task<Showtime> GetByIdAsync(Guid id);

        Task UpdateShowtimeAsync(UpdateShowtimeDto updateShowtimeDto, Guid id);

        Task DeleteShowtimeAsync(Guid id);

        Task<bool> SaveChangesAsync();

    }
}