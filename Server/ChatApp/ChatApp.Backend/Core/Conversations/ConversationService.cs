using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Core.Conversations.DTOs;
using ChatApp.Backend.Core.Enums;
using ChatApp.Backend.Core.Messages.DTOs;
using ChatApp.Backend.Core.Users.DTOs;
using ChatApp.Backend.Domain;
using ChatApp.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Backend.Core.Conversations;

public class ConversationService : IConversationService
{
    private readonly ChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public ConversationService(ChatDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<ConversationListDto>>> GetUserConversations(string userId)
    {
        try
        {
            var groups = await _dbContext
                .Conversations.Where(c => c.Users.Any(u => u.Id == userId))
                .Include(c => c.Users)
                .Include(c => c.Messages.OrderByDescending(conv => conv.CreatedAt).Take(1))
                .ProjectTo<ConversationListDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return Result<List<ConversationListDto>>.Success(groups);
        }
        catch (Exception ex)
        {
            return Result<List<ConversationListDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result<SingleConversationDto>> GetConversation(int conversationId)
    {
        try
        {
            var conversation = await _dbContext
                .Conversations.Where(c => c.Id == conversationId)
                .Include(c => c.GroupUsers)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync();

            if (conversation == null)
            {
                return Result<SingleConversationDto>.Failure(
                    $"No conversation with id {conversationId}"
                );
            }
            var messages = await _dbContext
                .Messages.Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(50)
                .Include(m => m.MessageStatuses)
                .ToListAsync();

            conversation.Messages = messages;

            return Result<SingleConversationDto>.Success(
                _mapper.Map<SingleConversationDto>(conversation)
            );
        }
        catch (Exception ex)
        {
            return Result<SingleConversationDto>.Failure(ex.Message);
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

    public async Task<Result<bool>> CheckIfUserInConversation(int conversationId, string userId)
    {
        var isUserInConversation = await _dbContext.ConversationUsers.AnyAsync(cu =>
            cu.ConversationId == conversationId && cu.UserId == userId
        );
        return isUserInConversation
            ? Result<bool>.Success(isUserInConversation)
            : Result<bool>.Failure("Cannot access conversations the user is not a part of");
    }
}
