using TheaterService.Models;

namespace TheaterService.Data
{
    public interface IShowtimeRepository
    {
        Task AddShowtimeAsync(Showtime showtime);

        Task<IEnumerable<Showtime>> GetAllShowtimesAsync();

        void UpdateShowtime(Showtime showtime);

        void DeleteShowtime(Showtime showtime);

    }
}