using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Core.Enums;
using ChatApp.Backend.Domain;
using ChatApp.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Core.Messages;

public class MessageService : IMessageService
{
    private readonly ChatDbContext _dbContext;

    public MessageService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // public async Task<Result<int>> CreateMessageAsync(
    //     string senderId,
    //     string receiverUserId,
    //     string content
    // )
    // {
    //     try
    //     {
    //         var message = new Message { SenderId = senderId, Content = content };
    //         await _dbContext.Messages.AddAsync(message);
    //         var messageReceiver = new MessageReceivers
    //         {
    //             MessageId = message.Id,
    //             UserId = receiverUserId,
    //             Status = MessageStatus.Read,
    //         };
    //         await _dbContext.MessageReceivers.AddAsync(messageReceiver);
    //         await _dbContext.SaveChangesAsync();
    //         return Result<int>.Success(message.Id);
    //     }
    //     catch (DbUpdateException ex)
    //     {
    //         return Result<int>.Failure(
    //             "An error occurred while inserting the message: " + ex.Message
    //         );
    //     }
    //     catch (Exception ex)
    //     {
    //         return Result<int>.Failure("An unexpected error occurred: " + ex.Message);
    //     }
    // }

    public async Task<Result<int>> CreateMessageAsync(
        string senderId,
        int conversationId,
        string content
    )
    {
        var conversationUserIds = await _dbContext
            .ConversationUsers.Where(cu => cu.ConversationId == conversationId)
            .Select(cu => cu.UserId)
            .AsNoTracking()
            .ToListAsync();

        if (conversationUserIds.IsNullOrEmpty())
        {
            return Result<int>.Failure($"No conversation exists with id: {conversationId}");
        }
        try
        {
            var message = new Message { SenderId = senderId, Content = content };
            await _dbContext.Messages.AddAsync(message);
            var userReceivers = conversationUserIds
                .Select(userId => new MessageReceivers
                {
                    MessageId = message.Id,
                    UserId = userId,
                    Status = MessageStatus.Sent,
                })
                .ToList();

            await _dbContext.MessageReceivers.AddRangeAsync(userReceivers);
            await _dbContext.SaveChangesAsync();
            return Result<int>.Success(message.Id);
        }
        catch (DbUpdateException ex)
        {
            return Result<int>.Failure(
                "An error occurred while inserting the message: " + ex.Message
            );
        }
        catch (Exception ex)
        {
            return Result<int>.Failure("An unexpected error occurred: " + ex.Message);
        }
    }

    public async Task<Result<Unit>> UpdateDeliveredMessage(int messageId, string userReceiverId)
    {
        var message = await _dbContext.MessageReceivers.FindAsync(messageId, userReceiverId);

        if (message == null)
        {
            return Result<Unit>.Failure(
                $"No message with id: {message} and receiver with id: {userReceiverId} found in database"
            );
        }

        if (message.Status != MessageStatus.Sent)
        {
            return Result<Unit>.Failure("Message already marked as delivered");
        }

        try
        {
            message.Status = MessageStatus.Delivered;
            await _dbContext.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (DbUpdateException ex)
        {
            return Result<Unit>.Failure(
                "An error occurred while updating the message status: " + ex.Message
            );
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure("An unexpected error occurred: " + ex.Message);
        }
    }

    public async Task<Result<List<int>>> UpdateReadMessages(string senderId, string userReceiverId)
    {
        var messageIds = await _dbContext
            .MessageReceivers.Where(mr =>
                senderId == mr.Message.SenderId
                && mr.UserId == userReceiverId
                && mr.Status == MessageStatus.Delivered
            )
            .Select(mr => mr.MessageId)
            .ToListAsync();

        if (messageIds.Count == 0)
        {
            return Result<List<int>>.Failure(
                $"No delivered messages found for the sender with id: {senderId} "
                    + $"and receiver with id: {userReceiverId}."
            );
        }

        try
        {
            await _dbContext
                .MessageReceivers.Where(mr => messageIds.Contains(mr.MessageId))
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(mr => mr.Status, MessageStatus.Read)
                );
            return Result<List<int>>.Success(messageIds);
        }
        catch (DbUpdateException ex)
        {
            return Result<List<int>>.Failure(
                "An error occurred while updating the message status: " + ex.Message
            );
        }
        catch (Exception ex)
        {
            return Result<List<int>>.Failure("An unexpected error occurred: " + ex.Message);
        }
    }
}
