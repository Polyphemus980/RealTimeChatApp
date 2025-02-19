using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Infrastructure.Data;

namespace ChatApp.Backend.Core.Users;

public interface IUserService
{
    public Task<Result<bool>> IsNewUser(string? userId);

    public Task<Result<bool>> IsNameFree(string name);
    public Task<Result<string>> CreateUserAsync(string? userId, string? email, string? displayName);
}
