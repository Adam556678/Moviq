using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ReservationService.Data;
using ReservationService.DTOs;
using ReservationService.Models;
using ReservationService.Services;
using ReservationService.Services.AsyncDataService;
using ReservationService.Services.Events;

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

        [Authorize]
        public async Task<bool> ProcessPayment(
            Guid reservationId,
            [Service] IReservationRepo reservationRepo,
            [Service] IShowtimeService showtimeService,
            [Service] EventBusPublisher eventPublisher
        )
        {
            // Get reservation and showtime details
            var reservation = await reservationRepo.GetByIdAsync(reservationId);
            var showtime = await showtimeService.GetByIdAsync(reservation.ShowtimeId);

            if (reservation == null)
                throw new InvalidOperationException("Reservation with this ID does not exist.");
            if (showtime == null)
                throw new InvalidOperationException("Showtime for this reservation does not exist.");

            // SAFETY CHECK: Don't let them pay if the seat lock failed or hasn't arrived!
            if (reservation.Status == ReservationStatus.Cancelled)
                throw new GraphQLException("Seats are no longer available.");

            try
            {
                // Create ReservationCreated Event and publish
                var reservationCreated = new ReservationCreatedEvent
                {
                    ReservationId = reservation.Id,
                    ShowtimeStart = showtime.StartTime,
                    HallName = showtime.HallName,
                    MovieName = showtime.MovieName,
                    SeatIds = reservation.ReservedSeats
                        .Select(rs => rs.ShowtimeSeatId)
                        .ToList(),
                    TotalPrice = reservation.TotalAmount
                };

                await eventPublisher.PublishReservationCreated(reservationCreated);
                return true;
            }
            catch (System.Exception e)
            {
                throw new GraphQLException(e.Message);
            }
        }
    }
}