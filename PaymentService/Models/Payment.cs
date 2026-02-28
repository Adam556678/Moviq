namespace PaymentService.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public Guid ReservationId { get; set; }

        public string? StripeSessionId { get; set; } // The UI ID
        public string? StripePaymentIntentId { get; set; } // The Money ID

        public decimal Price { get; set; }
        public string Currency { get; set; } = "usd";
        public int Quantity { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum PaymentStatus
    {
        Pending,
        Succeeded,
        Failed,
        Refunded
    }
}