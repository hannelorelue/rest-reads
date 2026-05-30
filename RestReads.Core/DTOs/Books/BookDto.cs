namespace RestReads.Core.DTOs.Books;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public int? TotalPages { get; set; }
    public string? CoverUrl { get; set; }
}
