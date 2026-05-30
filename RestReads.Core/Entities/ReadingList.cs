using RestReads.Core.Enums;

namespace RestReads.Core.Entities;

public class ReadingList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public ListType Type { get; set; }
    public bool IsCustom { get; set; }

    public ICollection<ReadingEntry> Entries { get; set; } = new List<ReadingEntry>();
}
