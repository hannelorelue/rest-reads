using RestReads.Core.DTOs.Users;

namespace RestReads.Core.Interfaces;

public interface IUserService
{
    Task<UserProfileDto?> GetProfileAsync(string username);
}
