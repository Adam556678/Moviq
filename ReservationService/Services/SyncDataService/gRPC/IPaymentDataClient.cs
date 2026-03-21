using PaymentService;

namespace ReservationService.Services.SyncDataService.gRPC
{
    public interface IPaymentDataClient
    {
        Task<PaymentResponse> CreateCheckoutSession(PaymentRequest request);
    }
}