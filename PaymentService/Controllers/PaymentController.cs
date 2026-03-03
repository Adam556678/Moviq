using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Services;
using PaymentService.Services.AsyncDataService;
using PaymentService.Services.Events;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.Controllers
{

    [ApiController]
    [Route("api/payments/webhook")]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentsService _paymentService;
        private readonly EventBusPublisher _eventPublisher;

        public PaymentController(
            IConfiguration configuration, 
            IPaymentsService paymentService,
            EventBusPublisher eventPublisher
            )
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _eventPublisher = eventPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _configuration["Stripe:WebhookSecret"]
            );

            var session = stripeEvent.Data.Object as Session;
            var reservationId = Guid.Parse(session!.Metadata["reservationId"]);

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    {

                    // Update payment status
                    await _paymentService.UpdatePaymentStatusAsync(
                        reservationId, PaymentStatus.Succeeded);
                    
                    // broadcast the event
                    var evnt = new PaymentStatusUpdatedEvent
                    {
                        ReservationId = reservationId,
                        Status = PaymentStatus.Succeeded
                    };

                    await _eventPublisher.PublishEvent(evnt, routingKey: "payment.succeeded");
                    break;
                }
                case "checkout.session.expired":
                {
                    await _paymentService.UpdatePaymentStatusAsync(
                        reservationId, PaymentStatus.Expired);
                    
                    var evnt = new PaymentStatusUpdatedEvent
                    {
                        ReservationId = reservationId,
                        Status = PaymentStatus.Expired
                    };
                    await _eventPublisher.PublishEvent(evnt, routingKey: "payment.expired");

                    break;
                }
                case "payment_intent.payment_failed":
                    {
                        await _paymentService.UpdatePaymentStatusAsync(
                            reservationId, PaymentStatus.Failed
                        );

                        var evnt = new PaymentStatusUpdatedEvent
                        {
                            ReservationId = reservationId,
                            Status = PaymentStatus.Failed
                        };
                        await _eventPublisher.PublishEvent(evnt, routingKey: "payment.failed");

                        break;
                    }

                // TODO: Handle "charge.refunded" case

                default:
                    break;
            }

            return Ok();
        }
    }
}