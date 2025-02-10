using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Core.Common;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Core.Services;

public class AuthService : IAuthService
{
    public async Task<Result<string>> VerifyTokenAsync(string token)
    {
        if (token.IsNullOrEmpty())
        {
            return Result<string>.Failure(errorMessage: "Token is missing");
        }

        try
        {
            FirebaseToken verifiedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(
                token
            );
            return Result<string>.Success(verifiedToken.Uid);
        }
        catch (FirebaseAuthException ex)
        {
            return Result<string>.Failure(
                errorMessage: ex.Message,
                statusCode: StatusCodes.Status401Unauthorized
            );
        }
    }
}
