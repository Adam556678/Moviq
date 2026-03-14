using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.Models;

namespace ReservationService.Services
{
    public class ShowtimeService : IShowtimeService
    {

        private readonly AppDbContext _context;

        public ShowtimeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateShowtimeAsync(Showtime showtime)
        {
            await _context.Showtimes.AddAsync(showtime);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShowtimeAsync(Guid showtimeId)
        {

            var showtime = await _context.Showtimes
                .FirstOrDefaultAsync(s => s.ShowtimeId == showtimeId);
            
            if (showtime == null)
                throw new Exception("Showtime does not exist");

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();
        }

        public async Task<Showtime?> GetByIdAsync(Guid id)
        {
            return await _context.Showtimes.FirstOrDefaultAsync(
                st => st.ShowtimeId == id);
        }
    }
}