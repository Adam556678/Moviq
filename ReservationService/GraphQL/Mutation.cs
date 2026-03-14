using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ReservationService.Data;
using ReservationService.DTOs;
using ReservationService.Models;

namespace ReservationService.GraphQL
{
    public class Mutation
    {

        [Authorize]
        public async Task<Reservation> MakeReservation(
            MakeReservationDto input,
            [Service] IReservationRepo reservationRepo,
            ClaimsPrincipal user
        )
        {
            try
            {
                // Get user id from Token Claims
                var currentUserId = Guid.Parse(
                    user.FindFirst(ClaimTypes.NameIdentifier)!.Value
                );

                var reservation = await reservationRepo.MakeReservation(input, currentUserId);

                return reservation;
            }
            catch (System.Exception e)
            {
                throw new GraphQLException($"Error making reservation: {e.Message}");
            }
        }
    }
}