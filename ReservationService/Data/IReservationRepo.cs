using ReservationService.Models;

namespace ReservationService.Data
{
    public interface IReservationRepo
    {
        Task MakeReservation();

        Task<Reservation> GetReservationDetails(Guid reservationId);

        Task<bool> IsShowtimeExist(Guid showtimeId);

    }
}