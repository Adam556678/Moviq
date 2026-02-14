using System.ComponentModel.DataAnnotations;

namespace MoviesService.Models
{
    public class Movie
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public string? Synopsis { get; set; }

        [Required]
        public required DateTime ReleaseDate { get; set; }

        [Required]
        public required int Duration { get; set; }

        public string? Language { get; set; }

        [Required]
        public required ICollection<Genre> Genres { get; set; }

    }
}