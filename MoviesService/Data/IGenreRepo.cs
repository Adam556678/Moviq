using MoviesService.Models;

namespace MoviesService.Data
{
    public interface IGenreRepo
    {
        Task AddGenreAsync(Genre genre);

        Task<IEnumerable<Genre>> GetAllGenresAsync();

        void DeleteGenre(Genre genre);

        Task<bool> SaveChangesAsync();

        Task<Genre?> GetGenreByIdAsync(int id);

    }    
}