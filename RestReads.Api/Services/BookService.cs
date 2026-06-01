using Microsoft.EntityFrameworkCore;
using RestReads.Api.Data;
using RestReads.Core.DTOs.Books;
using RestReads.Core.DTOs.Common;
using RestReads.Core.Entities;
using RestReads.Core.Interfaces;

namespace RestReads.Api.Services;

public class BookService(AppDbContext db) : IBookService
{
    public async Task<PagedResult<BookDto>> GetAllAsync(int page, int pageSize)
    {
        var query = db.Books.AsNoTracking();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => ToDto(b))
            .ToListAsync();

        return new PagedResult<BookDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        return book is null ? null : ToDto(book);
    }

    public async Task<BookDto> CreateAsync(CreateBookRequest request)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Isbn = request.Isbn,
            OpenLibraryKey = request.OpenLibraryKey,
            TotalPages = request.TotalPages,
            CoverUrl = request.CoverUrl,
            CreatedAt = DateTime.UtcNow
        };

        db.Books.Add(book);
        await db.SaveChangesAsync();
        return ToDto(book);
    }

    public async Task<BookDto?> UpdateAsync(int id, CreateBookRequest request)
    {
        var book = await db.Books.FindAsync(id);
        if (book is null) return null;

        book.Title = request.Title;
        book.Author = request.Author;
        book.Isbn = request.Isbn;
        book.OpenLibraryKey = request.OpenLibraryKey;
        book.TotalPages = request.TotalPages;
        book.CoverUrl = request.CoverUrl;

        await db.SaveChangesAsync();
        return ToDto(book);
    }

    public async Task<(BookDto book, bool alreadyExists)> ImportAsync(ImportBookRequest request)
    {
        var existing = await db.Books.FirstOrDefaultAsync(b => b.OpenLibraryKey == request.OpenLibraryKey);
        if (existing is not null)
            return (ToDto(existing), true);

        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Isbn = request.Isbn,
            OpenLibraryKey = request.OpenLibraryKey,
            TotalPages = request.TotalPages,
            CoverUrl = request.CoverUrl,
            CreatedAt = DateTime.UtcNow
        };

        db.Books.Add(book);
        await db.SaveChangesAsync();
        return (ToDto(book), false);
    }

    private static BookDto ToDto(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Isbn = book.Isbn,
        TotalPages = book.TotalPages,
        CoverUrl = book.CoverUrl
    };
}
