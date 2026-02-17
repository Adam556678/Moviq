using TheaterService.Models;

namespace TheaterService.Data
{
    public class ShowtimeRepository : IShowtimeRepository
    {

        private readonly AppDbContext _context;

        public ShowtimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddShowtimeAsync(Showtime showtime)
        {
            // Check if start time is valid 
            if (DateTime.UtcNow > showtime.StartTime)
                throw new Exception("Invalid show start time");
            
            // Check if movie exists
            if (!_context.)
        }

        public void DeleteShowtime(Showtime showtime)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Showtime>> GetAllShowtimesAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateShowtime(Showtime showtime)
        {
            throw new NotImplementedException();
        }
    }
}