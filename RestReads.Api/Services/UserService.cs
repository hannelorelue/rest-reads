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
        return await db.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .Select(u => new UserProfileDto
            {
                Id = u.Id,
                Username = u.Username,
                CreatedAt = u.CreatedAt,
                ReadingLists = u.ReadingLists.Select(l => new ReadingListDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Type = l.Type,
                    IsCustom = l.IsCustom,
                    EntryCount = l.Entries.Count
                })
            })
            .FirstOrDefaultAsync();
    }
}
