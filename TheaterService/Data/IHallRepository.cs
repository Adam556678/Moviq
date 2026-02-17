using TheaterService.Models;

namespace TheaterService.Data
{
    public interface IHallRepository
    {
        Task AddHallAsync(Hall hall);

        void EditHall(Hall hall);

        Task AddSeatAsync(Seat seat);

        void EditSeat(Seat seat);

        void DeleteHall(Hall hall);

        void DeleteSeat(Seat seat);

        Task<bool> SaveChangesAsync();

    }
}