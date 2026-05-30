using RestReads.Core.DTOs.Books;
using RestReads.Core.DTOs.Common;

namespace RestReads.Core.Interfaces;

public interface IBookService
{
    Task<PagedResult<BookDto>> GetAllAsync(int page, int pageSize);
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(CreateBookRequest request);
    Task<BookDto?> UpdateAsync(int id, CreateBookRequest request);
}
