using ChatApp.Backend.Core.Common;

namespace ChatApp.Backend.Core.Authentication;

public interface IAuthService
{
    public Task<Result<string>> VerifyTokenAsync(string token);
}
