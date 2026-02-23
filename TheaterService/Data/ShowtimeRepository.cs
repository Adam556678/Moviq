using Microsoft.EntityFrameworkCore;
using TheaterService.Models;
using TheaterService.Services;

namespace TheaterService.Data
{
    public class ShowtimeRepository : IShowtimeRepository
    {

        private readonly AppDbContext _context;
        private readonly IMoviesService _movieService;
        private readonly IShowtimeSeatService _showtimeSeatService;

        public ShowtimeRepository(
            AppDbContext context,
            IMoviesService moviesService,
            IShowtimeSeatService showtimeSeatService
        )
        {
            _context = context;
            _movieService = moviesService;
            _showtimeSeatService = showtimeSeatService;
        }

        public async Task AddShowtimeAsync(Showtime showtime)
        {
            // Check if start time is valid 
            if (DateTime.UtcNow > showtime.StartTime)
                throw new InvalidOperationException("Invalid show start time");
            
            // Check if movie exists
            if (! await _movieService.IsMovieExistAsync(showtime.MovieId))
                throw new InvalidOperationException("Movie does not exist");

            // Load Hall and check if it exists
            var hall = await _context.Halls
                .Include(h => h.Seats)
                .FirstOrDefaultAsync(h => h.Id == showtime.HallId);

            if (hall == null)
                throw new InvalidOperationException("Hall does not exist");

            showtime.Id = Guid.NewGuid();
            showtime.Hall = hall;

            // Initialize showtime seats
            _showtimeSeatService.InitializeSeatsForShowtime(showtime);

            // Add showtime to DB 
            await _context.Showtimes.AddAsync(showtime);
        }

        public async Task DeleteShowtimeAsync(Showtime showtime)
        {
            var existing = await _context.Showtimes.FindAsync(showtime.Id);

            if (existing != null){
                _context.Showtimes.Remove(showtime);
            }
        }

        public async Task<IEnumerable<Showtime>> GetAllShowtimesAsync()
        {
            var showtimes = await _context.Showtimes
                .Include(s => s.Hall)
                .Include(s => s.SeatStates)
                .ToListAsync();
            
            return showtimes;
        }

        public void UpdateShowtime(Showtime showtime)
        {
            _context.Showtimes.Update(showtime);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}