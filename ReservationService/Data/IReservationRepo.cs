using ReservationService.DTOs;
using ReservationService.Models;

namespace ReservationService.Data
{
    public interface IReservationRepo
    {
        Task<Reservation> MakeReservation(MakeReservationDto reservationDto, Guid userId);

        Task<Reservation> GetReservationDetails(Guid reservationId);

    }
}