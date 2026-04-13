using ReservationService.DTOs;
using ReservationService.Models;

namespace ReservationService.Data
{
    public interface IReservationRepo
    {
        Task<Reservation> MakeReservation(MakeReservationDto reservationDto, Guid userId);

        Task UpdateReservationStatus(Guid reservationId, ReservationStatus status);

        Task<Reservation> GetByIdAsync(Guid reservationId);

    }
}