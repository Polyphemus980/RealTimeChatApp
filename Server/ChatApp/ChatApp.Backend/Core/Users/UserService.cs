using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Domain;
using ChatApp.Backend.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Core.Users;

public class UserService : IUserService
{
    private readonly ChatDbContext _dbContext;
    private const int MaxNameLength = 25;

    public UserService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsNewUser(string userId)
    {
        var userExists = await _dbContext.Users.AnyAsync(u => u.Id.Equals(userId));
        return !userExists;
    }

    public async Task<Result<string>> CreateUserAsync(string email, string displayName)
    {
        if (email.IsNullOrEmpty())
        {
            return Result<string>.Failure(errorMessage: "Email is empty or null");
        }

        if (displayName.IsNullOrEmpty())
        {
            return Result<string>.Failure(errorMessage: "Display name is empty or null");
        }

        if (displayName.Length > MaxNameLength)
        {
            return Result<string>.Failure(
                errorMessage: "Display name has length over the limit (25)"
            );
        }

        try
        {
            var newUser = new User { Email = email, DisplayName = displayName };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return Result<string>.Success(newUser.Id);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException { Number: 2627 or 2601 })
            {
                return Result<string>.Failure(
                    errorMessage: "An account with this email already exists"
                );
            }
            return Result<string>.Failure(
                errorMessage: $"A database error occurred: {ex.Message}",
                statusCode: 500
            );
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(
                errorMessage: $"Unexpected error occured while creating a user: {ex.Message})",
                statusCode: 500
            );
        }
    }
}
