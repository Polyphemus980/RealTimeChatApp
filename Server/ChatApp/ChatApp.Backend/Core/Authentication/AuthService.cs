using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Core.Services.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Core.Services;

public class AuthService : IAuthService
{
    public async Task<AuthResult> VerifyTokenAsync(string token)
    {
        if (token.IsNullOrEmpty())
        {
            return new AuthResult(IsValid: false, ErrorMessage: "Token is missing");
        }

        try
        {
            FirebaseToken verifiedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(
                token
            );
            return new AuthResult(IsValid: true, UserId: verifiedToken.Uid);
        }
        catch (FirebaseAuthException ex)
        {
            return new AuthResult(IsValid: false, ErrorMessage: ex.Message);
        }
    }
}
