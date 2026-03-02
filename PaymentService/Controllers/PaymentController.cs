using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Services;
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

        public PaymentController(IConfiguration configuration, IPaymentsService paymentService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
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

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    var session = stripeEvent.Data.Object as Session;
                    var reservationId = Guid.Parse(session!.Metadata["reservationId"]);
                    
                    // Update payment status
                    await _paymentService.UpdatePaymentStatusAsync(
                        reservationId, PaymentStatus.Succeeded);
                    
                    // TODO: broadcast the event

                    break;
                case "checkout.session.expired":

                case "payment_intent.payment_failed":

                // TODO: Handle "charge.refunded" case

                default:
                    break;
            }

            return Ok();
        }
    }
}