using Microsoft.EntityFrameworkCore;
using TheaterService.Models;

namespace TheaterService.Data
{
    public class HallRepository : IHallRepository
    {

        private readonly AppDbContext _context;

        public HallRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddHallAsync(Hall hall)
        {
            if (_context.Halls.Any(h => h.Name == hall.Name))
                throw new Exception("Hall with name already exists");
                
            await _context.Halls.AddAsync(hall);
        }

        public async Task AddSeatAsync(Seat seat)
        {
            var hall = await _context.Halls
                .Include(h => h.Seats)
                .FirstOrDefaultAsync(h => h.Id == seat.HallId);

            if (hall == null)
                throw new Exception("Hall doesn't exist");

            if (hall.Seats.Any(s => s.Row == seat.Row && s.Column == seat.Column))
                throw new Exception("Seat in this place already exists");

            await _context.Seats.AddAsync(seat);
        }

        public void DeleteHall(Hall hall)
        {
            _context.Halls.Remove(hall);
        }

        public void DeleteSeat(Seat seat)
        {
            _context.Seats.Remove(seat);
        }

        public void EditHall(Hall hall)
        {
            _context.Halls.Update(hall);
        }

        public void EditSeat(Seat seat)
        {
            _context.Seats.Update(seat);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}