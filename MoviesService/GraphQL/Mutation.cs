using MoviesService.Data;
using MoviesService.DTOs;
using MoviesService.Models;
using HotChocolate;
using HotChocolate.Authorization;

namespace MoviesService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] {"Admin"})]
        public async Task<Movie> AddMovie(
            AddMovieDto input,
            [Service] IMovieRepo movieRepo,
            [Service] IGenreRepo genreRepo
        )
        {
            // Create a movie entity with no genres
            var movie = new Movie
            {
              Title = input.Title,
              Synopsis = input.Synopsis,
              Duration = input.Duration,
              Language = input.Language,
              ReleaseDate = input.ReleaseDate,
              Genres = new List<Genre>()  
            };

            // fetch all genres and select the movie's genres
            var allGenres = await genreRepo.GetAllGenresAsync();
            var selectedGenres = allGenres.Where(g => input.GenreIds.Contains(g.Id)).ToList();

            if (selectedGenres.Count != input.GenreIds.Count)
                throw new GraphQLException("One or more genre IDs do not exist");

            foreach (var genre in selectedGenres)
            {
                movie.Genres.Add(genre);
            }

            await movieRepo.AddMovieAsync(movie);
            await movieRepo.SaveChangesAsync();
            
            return movie;
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<Genre> AddGenre(
            AddGenreDto input,
            [Service] IGenreRepo genreRepo
        )
        {
            // Create a genre entity
            var genre = new Genre
            {
                Name = input.Name,
                Movies = new List<Movie>()
            };

            await genreRepo.AddGenreAsync(genre);
            await genreRepo.SaveChangesAsync();

            return genre;
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<Movie> UpdateMovie(
            int id,
            UpdateMovieDto input,
            [Service] IMovieRepo movieRepo,
            [Service] IGenreRepo genreRepo
        )
        {
            // Get movie and check if exist
            var movie = await movieRepo.GetMovieByIdAsync(id);
            if (movie == null)
            {
                throw new GraphQLException("Movie Not Found");
            }

            // Update general fields
            if (input.Title != null)
                movie.Title = input.Title;
            if (input.Synopsis != null)
                movie.Synopsis = input.Synopsis;
            if (input.Duration != null)
                movie.Duration = input.Duration.Value;
            if (input.ReleaseDate.HasValue)
                movie.ReleaseDate = input.ReleaseDate.Value;
            if (input.Language != null)
                movie.Language = input.Language;

            // Update movie genres
            if (input.GenreIds != null){
                var allGenres = await genreRepo.GetAllGenresAsync();
                var selectedGenres = allGenres.Where(g => input.GenreIds.Contains(g.Id)).ToList();

                if (selectedGenres.Count != input.GenreIds.Count)
                    throw new GraphQLException("One or more genre IDs do not exist");

                movie.Genres.Clear();
                foreach (var genre in selectedGenres)
                {
                    movie.Genres.Add(genre);
                }
            }

            // Save changes and return
            await movieRepo.SaveChangesAsync();
            return movie;
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> DeleteMovie(int id, [Service] IMovieRepo movieRepo)
        {
            // get movie and check if exist
            var movie = await movieRepo.GetMovieByIdAsync(id);
            if (movie == null)
                throw new GraphQLException("Movie not found");
            
            movieRepo.DeleteMovie(movie);
            await movieRepo.SaveChangesAsync();
            return true;
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> DeleteGenre(int id, [Service] IGenreRepo genreRepo)
        {
            // get genre and check if exist
            var genre = await genreRepo.GetGenreByIdAsync(id);
            if (genre == null)
                throw new GraphQLException("Genre not found");
            
            genreRepo.DeleteGenre(genre);
            await genreRepo.SaveChangesAsync();
            return true;
        }
    }
}