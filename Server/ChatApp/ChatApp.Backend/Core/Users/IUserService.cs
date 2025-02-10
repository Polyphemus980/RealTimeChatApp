using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Infrastructure.Data;

namespace ChatApp.Backend.Core.Users;

public interface IUserService
{
    public Task<bool> IsNewUser(string userId);

    public Task<Result<string>> CreateUserAsync(string email, string displayName);
}
