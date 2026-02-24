using HotChocolate.Authorization;
using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.GraphQL
{
    [ExtendObjectType(typeof(Mutation))]
    public class ShowtimeMutation
    {
        [Authorize(Roles = new[] {"Admin"})]
        public async Task<Showtime> AddShowtime(
            AddShowtimeDto input,
            [Service] IShowtimeRepository showtimeRepository
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
                return showtime;
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> DeleteShowtime(
            Guid id,
            [Service] IShowtimeRepository showtimeRepository
        )
        {
            try
            {
                await showtimeRepository.DeleteShowtimeAsync(id);
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