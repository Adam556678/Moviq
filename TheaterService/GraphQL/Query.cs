using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Services.Caching;

namespace TheaterService.GraphQL
{
    public class Query
    {

        private readonly IRedisCacheService _cache;

        public Query(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<Hall>> GetAllHalls(
            [Service] IHallRepository hallRepository
        )
        {
            var halls = await hallRepository.GetAllHallsAsync();
            return halls;
        } 
        public async Task<IEnumerable<ShowtimeReadDto>> GetAllShowimes(
            [Service] IShowtimeRepository showtimeRepository
        )
        {
            // hit redis cache and return if exist
            var cachedShowtimes = await _cache.GetData<IEnumerable<ShowtimeReadDto>>("showtimes");
            if (cachedShowtimes is not null)
            {
                Console.WriteLine("--> Returned cached showtimes");
                return cachedShowtimes;
            }

            var showtimes = await showtimeRepository.GetAllShowtimesAsync();

            // Map showtimes to DTO
            var showtimeDtos = showtimes.Select(s => new ShowtimeReadDto
            {
                Id = s.Id,
                HallId = s.HallId,
                MovieId = s.MovieId,
                StartTime = s.StartTime,
                SeatStates = s.SeatStates.Select(st => new ShowtimeSeatDto
                {
                    Id = st.Id,
                    SeatId = st.SeatId,
                    ShowtimeId = st.ShowtimeId,
                    Status = st.Status,
                    LockExpiration = st.LockExpiration
                }).ToList()
            });

            // cache data
            await _cache.SetData("showtimes" ,showtimeDtos);

            return showtimeDtos;
        }
    }

}