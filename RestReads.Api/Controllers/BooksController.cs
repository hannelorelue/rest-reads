using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestReads.Core.DTOs.Books;
using RestReads.Core.DTOs.Common;
using RestReads.Core.Interfaces;

namespace RestReads.Api.Controllers;

[ApiController]
[Route("api/books")]
[Authorize]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<BookDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await bookService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await bookService.GetByIdAsync(id);
        return book is null ? NotFound() : Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<BookDto>> Create(CreateBookRequest request)
    {
        var book = await bookService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<BookDto>> Update(int id, CreateBookRequest request)
    {
        var book = await bookService.UpdateAsync(id, request);
        return book is null ? NotFound() : Ok(book);
    }
}
