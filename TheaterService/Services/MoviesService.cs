using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheaterService.Data;
using TheaterService.Models;

namespace TheaterService.Services
{

    public class MoviesService : IMoviesService
    {

        private readonly AppDbContext _context;

        public MoviesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            if (!_context.Movies.Any(m => m.Id == movie.Id))
                await _context.Movies.AddAsync(movie);
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            var existing = await _context.Movies.FindAsync(movie.Id);
            if (existing != null)
            {
                _context.Movies.Remove(existing);
            }
        }

        public async Task<bool> IsMovieExistAsync(Guid id)
        {
            return await _context.Movies.AnyAsync(m => m.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}