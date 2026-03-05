namespace TheaterService.Models
{
    public class TimePricing
    {
        public Guid Id { get; set; }

        public required string StartHour { get; set; }
        public required string EndHour { get; set; }

        public decimal Multiplier { get; set; }
    }
}