using MoviesService.Services.AsyncDataService.Events;

namespace MoviesService.Services.AsyncDataService
{
    public interface IEventBusClient
    {
        Task PublishNewMovie(MovieCreatedEvent movieCreatedEvent);

        Task PublishMovieDeleted(MovieDeletedEvent movieDeletedEvent);
    }
}