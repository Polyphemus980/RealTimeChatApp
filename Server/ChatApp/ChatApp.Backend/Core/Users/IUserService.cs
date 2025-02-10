using ChatApp.Backend.Infrastructure.Data;

namespace ChatApp.Backend.Core.Users;

public interface IUserService
{
    public Task<bool> IsNewUser(string userId);

    public Task<UserResult> CreateUser(string email, string displayName);
}
