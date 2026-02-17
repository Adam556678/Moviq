using MoviesService.Models;

namespace MoviesService.Data
{
    public interface IMovieRepo
    {
        // Get all movies
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        // Get a single movie by Id
        Task<Movie?> GetMovieByIdAsync(Guid id);

        // Add a new movie
        Task AddMovieAsync(Movie movie);

        // Update an existing movie
        void UpdateMovie(Movie movie);

        // Delete a movie
        void DeleteMovie(Movie movie);

        // Save changes to the database
        Task<bool> SaveChangesAsync();        
    }    
}