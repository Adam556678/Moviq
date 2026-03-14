namespace ReservationService.DTOs
{
    public class MakeReservationDto
    {
        public Guid ShowtimeId { get; set; }

        public ICollection<Guid> SeatsId { get; set; } = new List<Guid>();
    }
}