namespace TheaterService.Models
{
    public class TimePricing
    {
        public Guid Id { get; set; }

        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }

        public decimal Multiplier { get; set; }
    }
}