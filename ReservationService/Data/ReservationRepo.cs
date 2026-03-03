using ReservationService.Models;

namespace ReservationService.Data
{
    public class ReservationRepo : IReservationRepo
    {
        public Task<Reservation> GetReservationDetails(Guid reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsShowtimeExist(Guid showtimeId)
        {
            throw new NotImplementedException();
        }

        public Task MakeReservation()
        {
            throw new NotImplementedException();
        }

        private Task<decimal> CalculateTotalPrice()
        {
            throw new NotImplementedException();
        }
    }
}