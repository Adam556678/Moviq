using PaymentService.Services.Events;

namespace PaymentService.Services
{
    public interface IPaymentsService
    {
        Task SavePaymentAsync(ReservationCreatedEvent createdEvent);

        Task CreateCheckoutSessionAsync(ReservationCreatedEvent createdEvent);
    }
}