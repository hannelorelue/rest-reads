namespace RestReads.Core.Entities;

public class ReadingEntry
{
    public int Id { get; set; }
    public int ReadingListId { get; set; }
    public ReadingList ReadingList { get; set; } = null!;
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public int? CurrentPage { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
