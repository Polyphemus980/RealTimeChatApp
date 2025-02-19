using System.ComponentModel.DataAnnotations;
using ChatApp.Backend.Api.Controllers;
using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Domain;
using ChatApp.Backend.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Core.Users;

public class UserService : IUserService
{
    private readonly ChatDbContext _dbContext;
    private readonly IValidator<RegisterData> _validator;
    private const int MaxNameLength = 25;

    public UserService(ChatDbContext dbContext, IValidator<RegisterData> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<Result<bool>> IsNewUser(string? userId)
    {
        if (userId.IsNullOrEmpty())
        {
            return Result<bool>.Failure("User ID must not be empty");
        }
        try
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id.Equals(userId));
            return Result<bool>.Success(!userExists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> IsNameFree(string name)
    {
        try
        {
            var userWithNameExists = await _dbContext.Users.AnyAsync(u =>
                u.DisplayName.Equals(name)
            );
            return Result<bool>.Success(!userWithNameExists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<string>> CreateUserAsync(
        string? userId,
        string? email,
        string? displayName
    )
    {
        var registerData = new RegisterData(userId, email, displayName);
        var validationResult = await _validator.ValidateAsync(registerData);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(
                ", ",
                validationResult.Errors.Select(e => e.ErrorMessage)
            );
            return Result<string>.Failure(errorMessage: errorMessages);
        }

        try
        {
            var newUser = new User
            {
                Id = userId!,
                Email = email!,
                DisplayName = displayName!,
            };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return Result<string>.Success(newUser.Id);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException { Number: 2627 or 2601 })
            {
                if (ex.Message.Contains("IX_UNIQUE_EMAIL"))
                {
                    return Result<string>.Failure("An account with this email already exists");
                }
                if (ex.Message.Contains("IX_UNIQUE_NAME"))
                {
                    return Result<string>.Failure("Display name is already taken");
                }
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
