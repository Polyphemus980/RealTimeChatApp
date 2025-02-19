using ChatApp.Backend.Core.Common;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Core.Authentication;

public class AuthService : IAuthService
{
    public async Task<Result<FirebaseToken>> VerifyTokenAsync(string? token)
    {
        if (token.IsNullOrEmpty())
        {
            return Result<FirebaseToken>.Failure(
                errorMessage: "Token is missing",
                statusCode: StatusCodes.Status401Unauthorized
            );
        }
        try
        {
            var verifiedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            return Result<FirebaseToken>.Success(verifiedToken);
        }
        catch (FirebaseAuthException ex)
        {
            Console.WriteLine(ex.Message);
            return Result<FirebaseToken>.Failure(
                errorMessage: ex.Message,
                statusCode: StatusCodes.Status401Unauthorized
            );
        }
    }
}
