namespace MoviesService.DTOs
{
    public record UpdateMovieDto(
        string? Title,
        string? Synopsis, 
        int? Duration, 
        DateTime? ReleaseDate,
        string? Language,
        List<Guid>? GenreIds
    );
}