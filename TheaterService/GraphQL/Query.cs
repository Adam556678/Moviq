using TheaterService.Data;
using TheaterService.Models;

namespace TheaterService.GraphQL
{
    public class Query
    {
        public async Task<IEnumerable<Hall>> GetAllHalls(
            [Service] IHallRepository hallRepository
        )
        {
            var halls = await hallRepository.GetAllHallsAsync();
            return halls;
        } 
    }
}