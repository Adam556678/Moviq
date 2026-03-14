using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.Enums;

namespace ReservationService.Services
{
    public class SeatService : ISeatService
    {

        private readonly AppDbContext _context;

        public SeatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsSeatExists(Guid showtimeSeatId)
        {
            return await _context.SeatPricings
                .AnyAsync(s => s.SeatId == showtimeSeatId);
        }

        public Task UpdateSeatStatus(Guid showtimeSeatId, SeatStatus seatStatus)
        {
            throw new NotImplementedException();
        }
    }
}