
using Microsoft.EntityFrameworkCore;
using TheaterService.Data;
using TheaterService.Enums;
using TheaterService.Models;

namespace TheaterService.Services
{
    public class ShowtimeSeatService : IShowtimeSeatService
    {

        private readonly AppDbContext _context;

        public ShowtimeSeatService(AppDbContext context)
        {
            _context = context;
        }

        public void InitializeSeatsForShowtime(Showtime showtime)
        {

            if (showtime.Hall?.Seats == null || !showtime.Hall.Seats.Any())
                throw new InvalidOperationException("Hall seats are not loaded");

            // create ShowtimeSeat for each hall seat
            foreach (var seat in showtime.Hall.Seats)
            {
                showtime.SeatStates.Add(new ShowtimeSeat
                {
                    Id = Guid.NewGuid(),
                    SeatId = seat.Id,
                    ShowtimeId = showtime.Id,
                    Status = SeatState.Available,
                });
            }
        }
        public Task HandleSeatReleaseRequest()
        {
            throw new NotImplementedException();
        }

        public Task ReserveSeat()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TryLockSeatAsync(Guid showtimeId, List<Guid> seatIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Fetch seats that are currently Available OR have an Expired lock
                var seatsToLock = await _context.ShowtimeSeats
                    .Where(s => seatIds.Contains(s.Id) && s.ShowtimeId == showtimeId)
                    .Where(s => s.Status == SeatState.Available || 
                       (s.Status == SeatState.Locked && s.LockExpiration < DateTime.UtcNow))
                    .ToListAsync();

                // 2. If we didn't find ALL the seats requested, someone else got one first
                if (seatsToLock.Count != seatIds.Count)
                {
                    return false;
                }

                // 3. Apply the 10-minute lock
                foreach (var seat in seatsToLock)
                {
                    seat.Status = SeatState.Locked;
                    seat.LockExpiration = DateTime.UtcNow.AddMinutes(10);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}