using RestReads.Core.DTOs.Lists;

namespace RestReads.Core.DTOs.Users;

public class UserProfileDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ReadingListDto> ReadingLists { get; set; } = [];
}
