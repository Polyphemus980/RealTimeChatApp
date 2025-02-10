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

    public async Task<UserResult> CreateUser(string email, string displayName)
    {
        if (email.IsNullOrEmpty())
        {
            return new UserResult(
                IsValid: false,
                ErrorMessage: "Email is empty or null",
                StatusCode: StatusCodes.Status400BadRequest
            );
        }

        if (displayName.IsNullOrEmpty())
        {
            return new UserResult(
                IsValid: false,
                ErrorMessage: "Display name is empty or null",
                StatusCode: StatusCodes.Status400BadRequest
            );
        }

        if (displayName.Length > MaxNameLength)
        {
            return new UserResult(
                IsValid: false,
                ErrorMessage: "Display name has length over the limit (25)",
                StatusCode: StatusCodes.Status400BadRequest
            );
        }

        try
        {
            var newUser = new User() { Email = email, DisplayName = displayName };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return new UserResult(IsValid: true, UserId: newUser.Id);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException { Number: 2627 or 2601 })
            {
                return new UserResult(
                    false,
                    ErrorMessage: "Email is already in use",
                    StatusCode: 400
                );
            }
            return new UserResult(
                false,
                ErrorMessage: $"A database error occurred: {ex.Message}",
                StatusCode: 500
            );
        }
        catch (Exception ex)
        {
            return new UserResult(
                IsValid: false,
                ErrorMessage: $"Unexpected error occured while creating a user: {ex.Message})",
                StatusCode: 500
            );
        }
    }
}
