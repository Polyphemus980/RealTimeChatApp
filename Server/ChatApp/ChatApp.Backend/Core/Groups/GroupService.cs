using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Core.Enums;
using ChatApp.Backend.Domain;
using ChatApp.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Backend.Core.Groups;

public class ConversationService : IGroupService
{
    private readonly ChatDbContext _dbContext;

    public ConversationService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<Conversation>>> GetUserConversations(string userId)
    {
        try
        {
            var groups = await _dbContext
                .ConversationUsers.Where(g => g.UserId == userId)
                .Select(g => g.Conversation)
                .AsNoTracking()
                .ToListAsync();
            return Result<List<Conversation>>.Success(groups);
        }
        catch (Exception ex)
        {
            return Result<List<Conversation>>.Failure(ex.Message);
        }
    }

    public async Task<Result<Unit>> AddUserToGroup(int groupId, string userId)
    {
        var userAlreadyExists = await _dbContext.ConversationUsers.AnyAsync(g =>
            g.ConversationId == groupId
            && g.UserId == userId
            && g.Conversation.Type == ConversationType.Group
        );

        if (userAlreadyExists)
        {
            return Result<Unit>.Failure("User is already in the group.");
        }

        try
        {
            await _dbContext.ConversationUsers.AddAsync(
                new ConversationUsers
                {
                    ConversationId = groupId,
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
            var groupUser = await _dbContext.ConversationUsers.SingleOrDefaultAsync(g =>
                g.UserId == userId
                && g.ConversationId == groupId
                && g.Conversation.Type == ConversationType.Group
            );
            if (groupUser == null)
            {
                return Result<Unit>.Failure("No user with this ID belongs to this group");
            }
            _dbContext.ConversationUsers.Remove(groupUser);
            await _dbContext.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.Message);
        }
    }

    public async Task<Result<Unit>> ChangeNickname(
        int conversationId,
        string userId,
        string? newNickname
    )
    {
        if (newNickname is "")
        {
            return Result<Unit>.Failure("Nickname can't be empty");
        }
        try
        {
            var conversationUser = await _dbContext.ConversationUsers.SingleOrDefaultAsync(g =>
                g.UserId == userId && g.ConversationId == conversationId
            );
            if (conversationUser == null)
            {
                return Result<Unit>.Failure("No user with this ID belongs to this group");
            }
            conversationUser.Nickname = newNickname;
            await _dbContext.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.Message);
        }
    }
}
