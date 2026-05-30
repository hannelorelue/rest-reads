using RestReads.Core.DTOs.Lists;

namespace RestReads.Core.Interfaces;

public interface IListService
{
    Task<IEnumerable<ReadingListDto>> GetUserListsAsync(int userId);
    Task<ReadingListDto?> GetListAsync(int userId, int listId);
    Task UpdateListAsync(int userId, int listId, CreateListRequest request);
    Task<ReadingListDto> CreateCustomListAsync(int userId, CreateListRequest request);
    Task DeleteCustomListAsync(int userId, int listId);
    Task AddEntryAsync(int userId, int listId, AddEntryRequest request);
    Task RemoveEntryAsync(int userId, int listId, int entryId);
    Task UpdateProgressAsync(int userId, int listId, int entryId, UpdateProgressRequest request);
}
