
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Services.Events;
using Stripe.Checkout;

namespace PaymentService.Services
{
    public class StripePaymentService : IPaymentsService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SessionService _stripeSessionService;

        public StripePaymentService(
            AppDbContext context, 
            IConfiguration configuration,
            SessionService stripeSessionService 
        )
        {
            _context = context;
            _configuration = configuration;
            _stripeSessionService = stripeSessionService;
        }

        public async Task SavePaymentAsync(
            ReservationCreatedEvent createdEvent,
            Session session
        )
        {
            if (session == null)
                throw new Exception("Checkout session doesn't exist");

            var payment = new Payment
            {
                ReservationId = createdEvent.ReservationId,
                Price = createdEvent.Price,
                Currency = createdEvent.Currency,
                Quantity = createdEvent.Quantity,
                StripeSessionId = session.Id,
                StripePaymentIntentId = session.PaymentIntentId
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Session> CreateCheckoutSessionAsync(ReservationCreatedEvent createdEvent)
        {

            var showTime = createdEvent.ShowtimeStart.ToString("dd MMM yyyy, HH:mm");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {"card"},
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(createdEvent.Price*100),
                            Currency = createdEvent.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions { 
                                Name = createdEvent.MovieName, 
                                Description =  $"Showtime starts at: {showTime}"
                            }
                        },
                        Quantity = createdEvent.Quantity
                    }
                },
                Mode = "payment",
                // Pass the ReservationId in Metadata so Webhooks can find it later
                Metadata = new Dictionary<string, string> { { "reservationId", createdEvent.ReservationId.ToString() } },
                SuccessUrl = _configuration["Stripe:SuccessURL"] + $"?id={createdEvent.ReservationId}",
                CancelUrl = _configuration["Stripe:CancelURL"]
            };
            
            // Create Stripe session and return it
            var session = await _stripeSessionService.CreateAsync(options);
            return session;
        }

        public async Task UpdatePaymentStatusAsync(Guid reservationId, PaymentStatus status)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.ReservationId == reservationId);
            
            if (payment == null)
                throw new Exception("Payment with this reservation id not found");
            
            // update status & save db
            payment.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}