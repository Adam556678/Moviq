
using MoviesService.Data;
using HotChocolate.Data;
using MoviesService.Models;
using MoviesService.Services.Caching;
using MoviesService.DTOs;
using HotChocolate.Authorization;

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
        public async Task<Movie?> GetMovieById(Guid id, [Service] IMovieRepo repo)
            => await repo.GetMovieByIdAsync(id);
        
        public async Task<IEnumerable<Genre>> GetGenres([Service] IGenreRepo repo) 
            => await repo.GetAllGenresAsync();
    }

    [ExtendObjectType("Query")]
    public class AuthQuery
    {
        [Authorize]
        public string WhoAmI([Service] IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext?.User;
            var name = user?.Identity?.Name ?? "Unknown";
            var roles = user?.Claims
                .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(c => c.Value);
                
            return $"User: {name}, Roles: {string.Join(", ", roles)}";
        }
    }
}