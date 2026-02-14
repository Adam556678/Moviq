using Microsoft.EntityFrameworkCore;
using MoviesService.Models;


namespace MoviesService.Data
{
    public class GenreRepo : IGenreRepo
    {
        private readonly AppDbContext _context;

        public GenreRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddGenreAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
        }

        public void DeleteGenre(Genre genre)
        {
           _context.Genres.Remove(genre);
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _context.Genres
            .Include(g => g.Movies)
            .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}