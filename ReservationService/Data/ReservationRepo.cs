using ReservationService.DTOs;
using ReservationService.Models;
using ReservationService.Services;

namespace ReservationService.Data
{
    public class ReservationRepo : IReservationRepo
    {

        private readonly AppDbContext _context;
        private readonly IShowtimeService _showtimeService;
        private readonly ISeatService _seatService;
        private readonly IShowtimePricingService _pricingService;

        public ReservationRepo(
            AppDbContext context,
            IShowtimeService showtimeService,
            ISeatService seatService,
            IShowtimePricingService pricingService
        )
        {
            _context = context;
            _showtimeService = showtimeService;
            _seatService = seatService;
            _pricingService = pricingService;
        }

        public async Task<Reservation> GetReservationDetails(Guid reservationId)
        {
            var reservation =  await _context.Reservations
                .FindAsync(reservationId);
            
            if (reservation == null)
                throw new InvalidOperationException("Reservation with this id does not exist");

            return reservation;
        }

        public async Task MakeReservation(MakeReservationDto reservationDto, Guid userId)
        {

            // Check if showtime exists
            var showtimeExists = _showtimeService
                .GetByIdAsync(reservationDto.ShowtimeId);

            if (showtimeExists == null)
                throw new InvalidOperationException("Showtime does not exist");

            // check if seats exist
            foreach (var seatId in reservationDto.SeatsId)
            {
                if (! await _seatService.IsSeatExists(seatId))
                    throw new InvalidOperationException($"Seat with ID: {seatId} does not exist"); 
            }

            var reservation = new Reservation
            {
                ShowtimeId = reservationDto.ShowtimeId,
                UserId = userId,
                ReservedSeats = reservationDto.SeatsId
                    .Select(seatId => new ReservationSeat
                    {
                        ShowtimeSeatId = seatId
                    }).ToList()
            };

            // Get total price
            reservation.TotalAmount = await _pricingService
                .CalculateTotalPrice(reservationDto.SeatsId, reservationDto.ShowtimeId);
            
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }
    }
}