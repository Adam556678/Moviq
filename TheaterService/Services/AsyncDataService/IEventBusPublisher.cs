using TheaterService.Services.Events;

namespace TheaterService.Services.AsyncDataService
{
    public interface IEventBusPublisher
    {
        Task PublishShowtimePricing(ShowtimePricingPublishedEvent @event);
        Task PublishShowtimeCreated(ShowtimeCreatedEvent @event);
        Task PublishShowtimeDeleted(ShowtimeDeletedEvent @event);
    }
}