using ReservationService.DTOs;
using ReservationService.Models;
using ReservationService.Services;
using ReservationService.Services.AsyncDataService;
using ReservationService.Services.Events;

namespace ReservationService.Data
{
    public class ReservationRepo : IReservationRepo
    {

        private readonly AppDbContext _context;
        private readonly IShowtimeService _showtimeService;
        private readonly ISeatService _seatService;
        private readonly IShowtimePricingService _pricingService;

        private readonly EventBusPublisher _eventPublisher;

        public ReservationRepo(
            AppDbContext context,
            IShowtimeService showtimeService,
            ISeatService seatService,
            IShowtimePricingService pricingService,
            EventBusPublisher eventPublisher
        )
        {
            _context = context;
            _showtimeService = showtimeService;
            _seatService = seatService;
            _pricingService = pricingService;
            _eventPublisher = eventPublisher;
        }

        public async Task<Reservation> GetReservationDetails(Guid reservationId)
        {
            var reservation =  await _context.Reservations
                .FindAsync(reservationId);
            
            if (reservation == null)
                throw new InvalidOperationException("Reservation with this id does not exist");

            return reservation;
        }

        public async Task<Reservation> MakeReservation(MakeReservationDto reservationDto, Guid userId)
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
                    }).ToList(),
                // Get total price
                TotalAmount = await _pricingService
                    .CalculateTotalPrice(reservationDto.SeatsId, reservationDto.ShowtimeId)
            };

            // Publish seat lock event
            var seatStatusUpdatedRequest = new SeatStatusUpdateRequested
            {
                SeatIds = reservationDto.SeatsId,
                ShowtimeId = reservationDto.ShowtimeId,
                StatusRequest = StatusRequest.Lock
            };
            await _eventPublisher.PublishSeatLockingRequest(seatStatusUpdatedRequest);

            // Add to DB and save
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }
    }
}