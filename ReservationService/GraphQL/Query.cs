using ReservationService.Data;
using ReservationService.Models;

namespace ReservationService.GraphQL
{
    public class Query
    {
        public async Task<Reservation> GetReservation(
            Guid id,
            [Service] IReservationRepo reservationRepo
        )
        {
            try
            {
                var reservation = await reservationRepo.GetReservationDetails(id);
                return reservation;
            }
            catch (System.Exception e)
            {
                throw new GraphQLException($"Error fetching reservation: {e.Message}");
            }
        }
    }
}