using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Domain;
using ChatApp.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Backend.Core.Groups;

public class GroupService : IGroupService
{
    private readonly ChatDbContext _dbContext;

    public GroupService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<Group>>> GetUserGroups(string userId)
    {
        try
        {
            var groups = await _dbContext
                .GroupUsers.Where(g => g.UserId == userId)
                .Select(g => g.Group)
                .AsNoTracking()
                .ToListAsync();
            return Result<List<Group>>.Success(groups);
        }
        catch (Exception ex)
        {
            return Result<List<Group>>.Failure(ex.Message);
        }
    }

    public async Task<Result<Unit>> AddUserToGroup(int groupId, string userId)
    {
        var userAlreadyExists = await _dbContext.GroupUsers.AnyAsync(g =>
            g.GroupId == groupId && g.UserId == userId
        );

        if (userAlreadyExists)
        {
            return Result<Unit>.Failure("User is already in the group.");
        }

        try
        {
            await _dbContext.GroupUsers.AddAsync(
                new GroupUsers
                {
                    GroupId = groupId,
                    UserId = userId,
                    IsAdmin = false,
                }
            );
            await _dbContext.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.Message);
        }
    }

    public async Task<Result<Unit>> RemoveUserFromGroup(int groupId, string userId)
    {
        try
        {
            var groupUser = await _dbContext.GroupUsers.SingleOrDefaultAsync(g =>
                g.UserId == userId && g.GroupId == groupId
            );
            if (groupUser == null)
            {
                return Result<Unit>.Failure("No user with this ID belongs to this group");
            }
            _dbContext.GroupUsers.Remove(groupUser);
            await _dbContext.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.Message);
        }
    }

    public async Task<Result<Unit>> ChangeNickname(int groupId, string userId, string? newNickname)
    {
        if (newNickname is "")
        {
            return Result<Unit>.Failure("Nickname can't be empty");
        }
        try
        {
            var groupUser = await _dbContext.GroupUsers.SingleOrDefaultAsync(g =>
                g.UserId == userId && g.GroupId == groupId
            );
            if (groupUser == null)
            {
                return Result<Unit>.Failure("No user with this ID belongs to this group");
            }
            groupUser.Nickname = newNickname;
            await _dbContext.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.Message);
        }
    }
}
