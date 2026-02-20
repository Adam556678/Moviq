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
                for (int col = 1; col <= NumColumns; col++)
                {
                    Seats.Add(new Seat
                    {
                        Row = row,
                        Column = col
                    });
                }
            }
        }

        public void ValidateSeatPosition(int row, int column)
        {
            if (Seats.Any(s => s.Row == row && s.Column == column))
                throw new Exception("Seat in this place already exists");

            if (column > NumColumns 
                || row > NumRows
                || row < 0
                || column < 0)
                throw new Exception("Invalid seat position");
        }

    }
}