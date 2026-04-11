namespace TheaterService.DTOs.PricingDtos
{
    public class AddTimePricingDto
    {
        public required string StartHour { get; set; }
        public required string EndHour { get; set; }
        public decimal Multiplier { get; set; }
    }
}