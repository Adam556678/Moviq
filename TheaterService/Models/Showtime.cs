using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterService.Models
{
    public class Showtime
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public Guid HallId { get; set; }

        [ForeignKey("HallId")]
        public virtual Hall Hall { get; set; } = default!;

        // Relation to the availability states
        public virtual ICollection<ShowtimeSeat> SeatStates { get; set; } = new List<ShowtimeSeat>();
    }
}