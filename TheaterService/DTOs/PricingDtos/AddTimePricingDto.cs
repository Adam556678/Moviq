namespace TheaterService.DTOs.PricingDtos
{
    public class AddTimePricingDto
    {
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public decimal Multiplier { get; set; }
    }
}