using Microsoft.EntityFrameworkCore;
using RestReads.Api.Data;
using RestReads.Core.DTOs.Lists;
using RestReads.Core.DTOs.Users;
using RestReads.Core.Interfaces;

namespace RestReads.Api.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<UserProfileDto?> GetProfileAsync(string username)
    {
        var user = await db.Users
            .AsNoTracking()
            .Include(u => u.ReadingLists)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user is null) return null;

        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            ReadingLists = user.ReadingLists.Select(l => new ReadingListDto
            {
                Id = l.Id,
                Name = l.Name,
                Type = l.Type,
                IsCustom = l.IsCustom,
                EntryCount = l.Entries.Count
            })
        };
    }
}
