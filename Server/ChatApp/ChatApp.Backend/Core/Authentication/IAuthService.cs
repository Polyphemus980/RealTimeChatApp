using ChatApp.Backend.Core.Common;
using FirebaseAdmin.Auth;

namespace ChatApp.Backend.Core.Authentication;

public interface IAuthService
{
    public Task<Result<FirebaseToken>> VerifyTokenAsync(string? token);
}
