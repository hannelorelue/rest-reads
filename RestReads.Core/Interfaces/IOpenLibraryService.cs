using RestReads.Core.DTOs.Books;

namespace RestReads.Core.Interfaces;

public interface IOpenLibraryService
{
    Task<IEnumerable<OpenLibraryBookDto>> SearchAsync(string query);
}
