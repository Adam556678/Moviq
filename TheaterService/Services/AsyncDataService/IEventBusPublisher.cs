using TheaterService.Services.Events;

namespace TheaterService.Services.AsyncDataService
{
    public interface IEventBusPublisher
    {
        Task PublishShowtimePricingCreated(ShowtimePricingPublishedEvent @event);
        Task PublishShowtimeCreated(ShowtimeCreatedEvent @event);
        Task PublishShowtimeDeleted(ShowtimeDeletedEvent @event);
        Task PublishSeatStatusUpdateResponse(SeatStatusUpdateResponse @event);
    }
}