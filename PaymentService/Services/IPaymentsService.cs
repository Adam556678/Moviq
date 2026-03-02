using PaymentService.Models;
using PaymentService.Services.Events;
using Stripe.Checkout;

namespace PaymentService.Services
{
    public interface IPaymentsService
    {
        Task SavePaymentAsync(ReservationCreatedEvent createdEvent, Session session);

        Task<Session> CreateCheckoutSessionAsync(ReservationCreatedEvent createdEvent);

        Task UpdatePaymentStatusAsync(Guid reservationId, PaymentStatus status);
    }
}