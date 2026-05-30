using RestReads.Core.Enums;

namespace RestReads.Core.DTOs.Lists;

public class ReadingListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ListType Type { get; set; }
    public bool IsCustom { get; set; }
    public int EntryCount { get; set; }
}
