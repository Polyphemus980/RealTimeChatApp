using ChatApp.Backend.Domain;
using ChatApp.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Backend.Core.Users;

using FluentValidation;

public record RegisterData(string? UserId, string? Email, string? DisplayName);

public class UserValidator : AbstractValidator<RegisterData>
{
    public UserValidator(ChatDbContext context)
    {
        var dbContext = context;
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email cannot be empty.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MustAsync(
                async (email, cancellationToken) =>
                    !await dbContext.Users.AnyAsync(u => u.Email.Equals(email), cancellationToken)
            )
            .WithMessage("Email is already taken.");
        ;

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Display name cannot be empty.")
            .MaximumLength(25)
            .WithMessage("Display name must be less than 25 characters.")
            .MustAsync(
                async (displayName, cancellationToken) =>
                    !await dbContext.Users.AnyAsync(
                        u => u.DisplayName.Equals(displayName),
                        cancellationToken
                    )
            )
            .WithMessage("Display name is already taken");

        RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("User id must exist");
    }
}
