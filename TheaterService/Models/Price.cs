using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheaterService.Enums;

namespace TheaterService.Models
{
    public class Price
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required Guid ShowtimeId { get; set; }

        [ForeignKey("ShowtimeId")]
        public virtual Showtime Showtime { get; set; } = default!;

        [Required]
        public required Guid ShowtimeSeatId { get; set; }

        [ForeignKey("ShowtimeSeatId")]
        public ShowtimeSeat ShowtimeSeat { get; set; } = default!;

        [Required]
        public required HallType HallType { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [MaxLength(5)]
        public string Currency { get; set; } = "USD";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}