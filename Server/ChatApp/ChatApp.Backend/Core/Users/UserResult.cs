namespace ChatApp.Backend.Core.Users;

public record UserResult(
    bool IsValid,
    int StatusCode = StatusCodes.Status201Created,
    string? ErrorMessage = null,
    string? UserId = null
);
