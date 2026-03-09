using TheaterService.Services.Events;

namespace TheaterService.Services.AsyncDataService
{
    public interface IEventBusPublisher
    {
        Task PublishShowtimePricing(ShowtimePricingPublishedEvent evnt);
    }
}