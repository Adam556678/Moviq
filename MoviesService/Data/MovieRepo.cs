using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesService.Models;

namespace MoviesService.Data
{
    public class MovieRepo : IMovieRepo
    {
        private readonly AppDbContext _context;

        public MovieRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies
            .Include(m => m.Genres)
            .ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void UpdateMovie(Movie movie)
        {
            _context.Movies.Update(movie);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}