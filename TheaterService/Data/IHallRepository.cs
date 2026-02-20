using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Data
{
    public interface IHallRepository
    {
        Task AddHallAsync(Hall hall);

        Task<Hall> EditHall(UpdateHallDto input, Guid id);

        Task AddSeatAsync(Seat seat);

        Task<Seat> EditSeat(UpdateSeatDto input, Guid id);

        Task DeleteHall(Guid id);

        Task DeleteSeat(Guid id);

        Task<bool> SaveChangesAsync();

    }
}