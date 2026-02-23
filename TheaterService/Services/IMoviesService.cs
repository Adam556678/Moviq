using TheaterService.Models;

namespace TheaterService.Services
{
    public interface IMoviesService
    {
        Task AddMovieAsync(Movie movie);

        Task DeleteMovieAsync(Movie movie);

        Task<bool> IsMovieExistAsync(Guid id);

        Task<Movie> GetMovieByIdAsync(Guid id);

    }
}