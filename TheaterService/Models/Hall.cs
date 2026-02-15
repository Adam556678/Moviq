using TheaterService.Enums;

namespace TheaterService.Models
{
    public class Hall
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int TotalSeats { get; set; }

        public HallType HallType { get; set; } = HallType.Standard;

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    }
}