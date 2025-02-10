using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Domain;
using FirebaseAdmin.Auth;

namespace ChatApp.Backend.Core.Services.Interfaces;

public interface IAuthService
{
    public Task<AuthResult> VerifyTokenAsync(string token);
}
