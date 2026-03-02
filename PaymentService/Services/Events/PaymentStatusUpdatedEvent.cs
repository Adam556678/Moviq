using PaymentService.Models;

namespace PaymentService.Services.Events
{
    public class PaymentStatusUpdatedEvent
    {
        public Guid ReservationId { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }

}