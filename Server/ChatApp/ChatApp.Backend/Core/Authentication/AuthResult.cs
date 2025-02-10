namespace ChatApp.Backend.Core.Authentication;

public record AuthResult(bool IsValid, string? UserId = null, string? ErrorMessage = null);
