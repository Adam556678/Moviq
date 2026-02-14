
using MoviesService.Data;
using HotChocolate.Data;
using MoviesService.Models;
using MoviesService.Services.Caching;
using MoviesService.DTOs;

namespace MoviesService.GraphQL
{
    public class Query
    {

        private readonly IRedisCacheService _cache;

        public Query(IRedisCacheService cache)
        {
            _cache = cache;
        }
        
        public async Task<IEnumerable<ReadMovieDto>> GetMovies([Service] IMovieRepo repo){

            // return cached movies if exist
            var cachedMovies = await _cache.GetData<IEnumerable<ReadMovieDto>>("movies");
            if (cachedMovies is not null)
            {
                Console.WriteLine("--> Returned cached movies");
                return cachedMovies;
            }

            // get movies from db 
            var movies = await repo.GetAllMoviesAsync();

            // map movies to DTOs
            var movieDtos = movies.Select(m => new ReadMovieDto(
                Id: m.Id,
                Title: m.Title,
                Synopsis: m.Synopsis,
                ReleaseDate: m.ReleaseDate,
                Duration: m.Duration,
                Language: m.Language,
                Genres: m.Genres.Select(g => g.Name).ToList()
            )).ToList();
            await _cache.SetData("movies", movieDtos);

            return movieDtos;
        }
        public async Task<Movie?> GetMovieById(int id, [Service] IMovieRepo repo)
            => await repo.GetMovieByIdAsync(id);
        
        public async Task<IEnumerable<Genre>> GetGenres([Service] IGenreRepo repo) 
            => await repo.GetAllGenresAsync();
    }
}