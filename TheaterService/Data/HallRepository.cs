using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheaterService.DTOs;
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

            hall.ValidateSeatPosition(seat.Row, seat.Column);

            await _context.Seats.AddAsync(seat);
        }

        public async Task DeleteHall(Guid id)
        {
            var hall = await _context.Halls.FindAsync(id);

            if (hall == null)
                throw new InvalidOperationException("Hall not found");

            _context.Halls.Remove(hall);
        }

        public async Task DeleteSeat(Guid id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
                throw new InvalidOperationException("Seat not found");
                
            _context.Seats.Remove(seat);
        }

        public async Task<Hall> EditHall(UpdateHallDto input, Guid id)
        {
            var hall = await _context.Halls.FindAsync(id); 
            
            if (hall == null)
                throw new Exception("Hall not found");
            
            // update hall entity
            if (input.Name != null)
                hall.Name = input.Name;
            if (input.NumRows != null)
                hall.NumRows = input.NumRows.Value;
            if (input.NumColumns != null)
                hall.NumColumns = input.NumColumns.Value;
            if (input.HallType != null)
                hall.HallType = input.HallType.Value;

            _context.Halls.Update(hall);
            return hall;
        }

        public async Task<Seat> EditSeat(UpdateSeatDto input, Guid id)
        {
            var seat = await _context.Seats
                .Include(s => s.Hall)
                    .ThenInclude(h => h.Seats)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seat == null)
                throw new Exception("Seat does not exist");

            var newRow = input.Row ?? seat.Row;
            var newColumn = input.Column ?? seat.Column;

            seat.Hall.ValidateSeatPosition(newRow, newColumn);

            // update seat entity
            seat.Row = newRow;
            seat.Column = newColumn;
            if (input.IsFunctional != null)
                seat.IsFunctional = input.IsFunctional.Value;
            if (input.SeatType != null)
                seat.SeatType = input.SeatType.Value;

            _context.Seats.Update(seat);
            return seat;
        }

        public async Task<IEnumerable<Hall>> GetAllHallsAsync()
        {
            var halls = await _context.Halls
                .Include(h => h.Seats)
                .ToListAsync();

            return halls;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}