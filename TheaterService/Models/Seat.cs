using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheaterService.Enums;

namespace TheaterService.Models
{
    public class Seat
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required int Row { get; set; }

        [Required]
        public required int Column { get; set; }

        public SeatType SeatType { get; set; } = SeatType.Standard;
        
        public bool IsFunctional { get; set; } = true;

        public Guid HallId { get; set; }

        [ForeignKey("HallId")]
        public virtual Hall Hall { get; set; } = default!;
    }
}