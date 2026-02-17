using System.ComponentModel.DataAnnotations;

namespace MoviesService.Models
{
    public class Genre
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required ICollection<Movie> Movies { get; set; }
    }
}