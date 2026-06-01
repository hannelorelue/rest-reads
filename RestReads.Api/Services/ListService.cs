using Microsoft.EntityFrameworkCore;
using RestReads.Api.Data;
using RestReads.Core.DTOs.Lists;
using RestReads.Core.Entities;
using RestReads.Core.Enums;
using RestReads.Core.Interfaces;


namespace RestReads.Api.Services;

public class ListService(AppDbContext db) : IListService
{
    public async Task<IEnumerable<ReadingListDto>> GetUserListsAsync(int userId)
    {
        return await db.ReadingLists
            .AsNoTracking()
            .Where(l => l.UserId == userId)
            .Select(l => new ReadingListDto
            {
                Id = l.Id,
                Name = l.Name,
                Type = l.Type,
                IsCustom = l.IsCustom,
                EntryCount = l.Entries.Count
            })
            .ToListAsync();
    }

    public async Task<ReadingListDto?> GetListAsync(int userId, int listId)
    {
        return await db.ReadingLists
            .AsNoTracking()
            .Where(l => l.Id == listId && l.UserId == userId)
            .Select(l => new ReadingListDto
            {
                Id = l.Id,
                Name = l.Name,
                Type = l.Type,
                IsCustom = l.IsCustom,
                EntryCount = l.Entries.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateListAsync(int userId, int listId, CreateListRequest request)
    {
        var list = await db.ReadingLists.FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId)
            ?? throw new KeyNotFoundException("List not found.");

        list.Name = request.Name;
        await db.SaveChangesAsync();
    }

    public async Task<ReadingListDto> CreateCustomListAsync(int userId, CreateListRequest request)
    {
        var list = new ReadingList
        {
            UserId = userId,
            Name = request.Name,
            Type = ListType.Custom,
            IsCustom = true
        };

        db.ReadingLists.Add(list);
        await db.SaveChangesAsync();
        return new ReadingListDto
        {
            Id = list.Id,
            Name = list.Name,
            Type = list.Type,
            IsCustom = list.IsCustom,
            EntryCount = 0
        };
    }

    public async Task DeleteCustomListAsync(int userId, int listId)
    {
        var list = await db.ReadingLists.FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId)
            ?? throw new KeyNotFoundException("List not found.");

        if (!list.IsCustom)
            throw new InvalidOperationException("Default lists cannot be deleted.");

        db.ReadingLists.Remove(list);
        await db.SaveChangesAsync();
    }

    public async Task AddEntryAsync(int userId, int listId, AddEntryRequest request)
    {
        var list = await db.ReadingLists.FirstOrDefaultAsync(l => l.Id == listId && l.UserId == userId)
            ?? throw new KeyNotFoundException("List not found.");

        var entry = new ReadingEntry
        {
            ReadingListId = listId,
            BookId = request.BookId,
            AddedAt = DateTime.UtcNow
        };

        db.ReadingEntries.Add(entry);
        await db.SaveChangesAsync();
    }

    public async Task RemoveEntryAsync(int userId, int listId, int entryId)
    {
        var entry = await db.ReadingEntries
            .Include(e => e.ReadingList)
            .FirstOrDefaultAsync(e => e.Id == entryId && e.ReadingListId == listId && e.ReadingList.UserId == userId)
            ?? throw new KeyNotFoundException("Entry not found.");

        db.ReadingEntries.Remove(entry);
        await db.SaveChangesAsync();
    }

    public async Task UpdateProgressAsync(int userId, int listId, int entryId, UpdateProgressRequest request)
    {
        var entry = await db.ReadingEntries
            .Include(e => e.ReadingList)
            .FirstOrDefaultAsync(e => e.Id == entryId && e.ReadingListId == listId && e.ReadingList.UserId == userId)
            ?? throw new KeyNotFoundException("Entry not found.");

        entry.CurrentPage = request.CurrentPage;
        entry.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

}
