namespace MoviesService.DTOs
{
    public record AddMovieDto(
        string Title,
        string? Synopsis, 
        int Duration, 
        DateTime ReleaseDate,
        string? Language,
        List<int> GenreIds
    );
}