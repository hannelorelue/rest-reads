namespace RestReads.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "user"; // "user" or "admin"
    public DateTime CreatedAt { get; set; }

    public ICollection<ReadingList> ReadingLists { get; set; } = new List<ReadingList>();
}
