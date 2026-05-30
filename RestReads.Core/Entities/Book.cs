namespace RestReads.Core.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public string? OpenLibraryKey { get; set; }
    public int? TotalPages { get; set; }
    public string? CoverUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
