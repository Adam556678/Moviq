namespace MoviesService.DTOs
{
    public record ReadMovieDto(
    
        int Id,
        string Title,
        string? Synopsis,
        DateTime ReleaseDate,
        int Duration,
        string? Language,        
        List<string> Genres
    );
}