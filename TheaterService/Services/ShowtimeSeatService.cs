
using TheaterService.Enums;
using TheaterService.Models;

namespace TheaterService.Services
{
    public class ShowtimeSeatService : IShowtimeSeatService
    {
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
        
        public Task HandleSeatLockRequest()
        {
            throw new NotImplementedException();
        }

        public Task HandleSeatReleaseRequest()
        {
            throw new NotImplementedException();
        }

        public Task ReserveSeat()
        {
            throw new NotImplementedException();
        }
    }
}