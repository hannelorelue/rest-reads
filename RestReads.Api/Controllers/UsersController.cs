using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestReads.Core.DTOs.Users;
using RestReads.Core.Interfaces;

namespace RestReads.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<ActionResult<UserProfileDto>> GetProfile(string username)
    {
        var profile = await userService.GetProfileAsync(username);
        return profile is null ? NotFound() : Ok(profile);
    }
}
