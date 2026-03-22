using System.Security.Claims;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using PaymentService;
using ReservationService.Data;
using ReservationService.DTOs;
using ReservationService.Models;
using ReservationService.Services;
using ReservationService.Services.AsyncDataService;
using ReservationService.Services.Events;
using ReservationService.Services.SyncDataService.gRPC;

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
        public async Task<string> ProcessPayment(
            Guid reservationId,
            [Service] IReservationRepo reservationRepo,
            [Service] IShowtimeService showtimeService,
            [Service] IPaymentDataClient paymentDataClient
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
            if (reservation.Status == ReservationStatus.Pending)
                throw new GraphQLException("Seats are not locked, please try again.");
            if (reservation.Status == ReservationStatus.Cancelled)
                throw new GraphQLException("Seats are no longer available.");

            var request = new PaymentRequest
            {
                ReservationId = reservation.Id.ToString(),
                HallName = showtime.HallName,
                MovieName = showtime.MovieName,
                Amount = (double)reservation.TotalAmount,
                ShowtimeStart = Timestamp.FromDateTime(showtime.StartTime.ToUniversalTime())
            };

            foreach (var seat in reservation.ReservedSeats)
            {
                request.SeatIds.Add(seat.ShowtimeSeatId.ToString());
            }

            var response = await paymentDataClient.CreateCheckoutSession(request);

            if (response.Success)
                return response.StripeSessionUrl;
            
            throw new GraphQLException("Payment intialization failed.");
        }
    }
}