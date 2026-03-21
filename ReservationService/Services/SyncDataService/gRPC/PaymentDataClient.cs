using PaymentService;

namespace ReservationService.Services.SyncDataService.gRPC
{
    public class PaymentDataClient : IPaymentDataClient
    {
        private readonly PaymentGrpc.PaymentGrpcClient _client;

        public PaymentDataClient(
            PaymentGrpc.PaymentGrpcClient client
        )
        {
            _client = client;
        }
        public async Task<PaymentResponse> CreateCheckoutSession(PaymentRequest request)
        {
            try
            {
                Console.WriteLine($"--> Calling gRPC Service for Reservation: {request.ReservationId}");
                
                var response = await _client.CreateCheckoutSessionAsync(request);
                return response;
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"--> Could not call gRPC Server: {e.Message}");
                return new PaymentResponse{ Success = false };
            }
        }
    }
}