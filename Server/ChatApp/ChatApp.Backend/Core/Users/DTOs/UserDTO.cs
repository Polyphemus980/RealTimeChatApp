namespace ChatApp.Backend.Core.Users.DTOs;

public record UserDto(string UserId, string DisplayName, UserWithGroupDto? GroupUserData);
