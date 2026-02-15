// A model for seat states at a showtime

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheaterService.Enums;

namespace TheaterService.Models
{
    public class ShowtimeSeat
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required Guid SeatId { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; } = default!;

        [Required]
        public required Guid ShowtimeId { get; set; }

        [ForeignKey("ShowtimeId")]
        public virtual Showtime Showtime { get; set; } = default!;

        public SeatState Status { get; set; } = SeatState.Available;

        public DateTime? LockExpiration { get; set; }

    }
}