using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Domain;

namespace ChatApp.Backend.Core.Groups;

public interface IGroupService
{
    public Task<Result<List<Group>>> GetUserGroups(string userId);
    public Task<Result<Unit>> AddUserToGroup(int groupId, string userId);
    public Task<Result<Unit>> RemoveUserFromGroup(int groupId, string userId);

    public Task<Result<Unit>> ChangeNickname(int groupId, string userId, string? newNickname);
}
