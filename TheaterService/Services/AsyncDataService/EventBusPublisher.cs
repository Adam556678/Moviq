using TheaterService.Services.Events;

namespace TheaterService.Services.AsyncDataService
{
    public class EventBusPublisher : IEventBusPublisher
    {
        public Task PublishShowtimePricing(ShowtimePricingPublishedEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}