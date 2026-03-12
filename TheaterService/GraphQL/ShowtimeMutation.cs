using HotChocolate.Authorization;
using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Services;
using TheaterService.Services.AsyncDataService;
using TheaterService.Services.Events;

namespace TheaterService.GraphQL
{
    [ExtendObjectType(typeof(Mutation))]
    public class ShowtimeMutation
    {
        [Authorize(Roles = new[] {"Admin"})]
        public async Task<Showtime> AddShowtime(
            AddShowtimeDto input,
            [Service] IShowtimeRepository showtimeRepository,
            [Service] IEventBusPublisher eventBusPublisher,
            [Service] IPricingService pricingService
        )
        {
            try
            {
                var showtime = new Showtime
                {
                    HallId = input.HallId,
                    MovieId = input.MovieId,
                    StartTime = input.StartTime
                };
    
                await showtimeRepository.AddShowtimeAsync(showtime);
                await showtimeRepository.SaveChangesAsync();

                // get full showtime
                var fullShowtime = await showtimeRepository.GetByIdAsync(showtime.Id);

                // Publish ShowtimeCreated and SowtimePricingPublished event
                var showtimeCreatedEvent = new ShowtimeCreatedEvent
                {
                    ShowtimeId = fullShowtime.Id,
                    HallName = fullShowtime.Hall.Name,
                    MovieTitle = fullShowtime.Movie.Title,
                    StartTime = fullShowtime.StartTime
                };

                var seatPrices = await pricingService.CalculatePriceAsync(fullShowtime);
                if (seatPrices == null)
                    throw new GraphQLException("Showtime has no pricing");

                var showtimePricingEvent = new ShowtimePricingPublishedEvent{
                    ShowtimeId = fullShowtime.Id,
                    SeatPrices = seatPrices
                };

                await eventBusPublisher.PublishShowtimeCreated(showtimeCreatedEvent);
                await eventBusPublisher.PublishShowtimePricingCreated(showtimePricingEvent);

                return fullShowtime;
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> DeleteShowtime(
            Guid id,
            [Service] IShowtimeRepository showtimeRepository,
            [Service] IEventBusPublisher eventBusPublisher
        )
        {
            try
            {
                await showtimeRepository.DeleteShowtimeAsync(id);
                await showtimeRepository.SaveChangesAsync();

                // Publish showtime deleted event to RabbitMQ
                var @event = new ShowtimeDeletedEvent
                {
                    ShowtimeId = id
                };
                await eventBusPublisher.PublishShowtimeDeleted(@event);

                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> UpdateShowtime(
            Guid id,
            UpdateShowtimeDto input,
            [Service] IShowtimeRepository showtimeRepository
        )
        {
            try
            {
                await showtimeRepository.UpdateShowtimeAsync(input, id);
                await showtimeRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }
        }
    }
}