namespace ReservationService.Services.Events
{
    public class PaymentStatusUpdatedEvent
    {
        public Guid ReservationId { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }

    public enum PaymentStatus
    {
        Pending,
        Succeeded,
        Failed,
        Refunded,
        Expired
    }

}