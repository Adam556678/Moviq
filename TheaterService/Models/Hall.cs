using TheaterService.Enums;

namespace TheaterService.Models
{
    public class Hall
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int NumRows { get; set; }
        public int NumColumns { get; set; }

        public HallType HallType { get; set; } = HallType.Standard;

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public void InitializeSeats()
        {
            if (Seats.Any())
                throw new InvalidOperationException("Seats already initialized");

            // Initialization logic
            for (int row = 1; row <= NumRows; row++)
            {
                for (int col = 0; col <= NumColumns; col++)
                {
                    Seats.Add(new Seat
                    {
                        Row = row,
                        Column = col
                    });
                }
            }
        }

    }
}