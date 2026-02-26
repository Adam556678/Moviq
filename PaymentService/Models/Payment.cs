namespace PaymentService.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public Guid ReservationId { get; set; }

        public Guid? StripeSessionId { get; set; } // The UI ID
        public Guid? StripePaymentIntentId { get; set; } // The Money ID

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum PaymentStatus
    {
        Pending,
        Succeeded,
        Failed
    }
}