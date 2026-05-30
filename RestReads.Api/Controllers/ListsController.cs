using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestReads.Core.DTOs.Lists;
using RestReads.Core.Interfaces;

namespace RestReads.Api.Controllers;

[ApiController]
[Route("api/lists")]
[Authorize]
public class ListsController(IListService listService) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyLists()
    {
        var lists = await listService.GetUserListsAsync(UserId);
        return Ok(lists);
    }

    [HttpGet("{listId}")]
    public async Task<IActionResult> GetList(int listId)
    {
        var list = await listService.GetListAsync(UserId, listId);
        return list is null ? NotFound() : Ok(list);
    }

    [HttpPut("{listId}")]
    public async Task<IActionResult> UpdateList(int listId, CreateListRequest request)
    {
        try
        {
            await listService.UpdateListAsync(UserId, listId, request);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomList(CreateListRequest request)
    {
        var list = await listService.CreateCustomListAsync(UserId, request);
        return CreatedAtAction(nameof(GetList), new { listId = list.Id }, list);
    }

    [HttpDelete("{listId}")]
    public async Task<IActionResult> DeleteCustomList(int listId)
    {
        try
        {
            await listService.DeleteCustomListAsync(UserId, listId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{listId}/entries")]
    public async Task<IActionResult> AddEntry(int listId, AddEntryRequest request)
    {
        try
        {
            await listService.AddEntryAsync(UserId, listId, request);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{listId}/entries/{entryId}")]
    public async Task<IActionResult> RemoveEntry(int listId, int entryId)
    {
        try
        {
            await listService.RemoveEntryAsync(UserId, listId, entryId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{listId}/entries/{entryId}/progress")]
    public async Task<IActionResult> UpdateProgress(int listId, int entryId, UpdateProgressRequest request)
    {
        try
        {
            await listService.UpdateProgressAsync(UserId, listId, entryId, request);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
