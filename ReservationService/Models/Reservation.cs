namespace ReservationService.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid ShowtimeId { get; set; }
        public Guid UserId { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public ICollection<ReservationSeat> ReservedSeats { get; set; } = new List<ReservationSeat>();
    }

    public enum ReservationStatus
    {
        Pending,   // User is currently at the checkout
        Confirmed, // Payment successfully received
        Cancelled, // User clicked cancel or payment failed
        Expired    // 10 minutes passed without payment
    }
}