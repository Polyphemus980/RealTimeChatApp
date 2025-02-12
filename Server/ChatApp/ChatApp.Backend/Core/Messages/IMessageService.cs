using ChatApp.Backend.Core.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChatApp.Backend.Core.Messages;

public interface IMessageService
{
    public Task<Result<int>> CreateMessageAsync(
        string senderId,
        string receiverUserId,
        string content
    );

    public Task<Result<int>> CreateGroupMessageAsync(
        string senderId,
        int receiverGroupId,
        string content
    );
    public Task<Result<Unit>> UpdateDeliveredMessage(int messageId, string userReceiverId);
    public Task<Result<List<int>>> UpdateReadMessages(string senderId, string userReceiverId);
}
