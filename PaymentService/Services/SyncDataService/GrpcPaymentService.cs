using Grpc.Core;
using PaymentService.Services.Events;
using Stripe.Checkout;

namespace PaymentService.Services.SyncDataService
{
    public class GrpcPaymentService : PaymentGrpc.PaymentGrpcBase
    {
        private readonly IPaymentsService _paymentService;

        public GrpcPaymentService(IPaymentsService paymentsService)
        {
            _paymentService = paymentsService;
        }

        public override async Task<PaymentResponse> CreateCheckoutSession(
            PaymentRequest request,
            ServerCallContext context
        )
        {
            Console.WriteLine($"gRPC server received request for reservation: {request.ReservationId}");

            try
            {
                var reservationEvent = new ReservationCreatedEvent
                {
                    HallName = request.HallName,
                    MovieName = request.MovieName,
                    ReservationId = Guid.Parse(request.ReservationId),
                    SeatIds = request.SeatIds.Select(Guid.Parse).ToList(),
                    Currency = "usd",
                    ShowtimeStart = request.ShowtimeStart.ToDateTime(),
                    TotalPrice = (decimal)request.Amount
                };

                Session session = await _paymentService.CreateCheckoutSessionAsync(reservationEvent);
                await _paymentService.SavePaymentAsync(reservationEvent, session);

                return new PaymentResponse
                {
                    StripeSessionUrl = session.Url,
                    Success = true
                };
            }
            catch (System.Exception)
            {
                return new PaymentResponse
                {
                    Success = false
                };
            }
        }
    }
}