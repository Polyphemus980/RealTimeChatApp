using System.Security.Claims;
using ChatApp.Backend.Core.Connection;
using ChatApp.Backend.Core.Conversations;
using ChatApp.Backend.Core.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Backend.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IConnectionManager _connectionManager;
    private readonly IMessageService _messageService;
    private readonly ConversationService _conversationService;

    public ChatHub(
        IConnectionManager connectionManager,
        IMessageService messageService,
        ConversationService conversationService
    )
    {
        _connectionManager = connectionManager;
        _messageService = messageService;
        _conversationService = conversationService;
    }

    public async Task SendMessage(int conversationId, string message)
    {
        var senderId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _messageService.CreateMessageAsync(senderId, conversationId, message);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't send a message " + result.ErrorMessage);
        }

        await Clients
            .Group(GetGroupNameFromId(conversationId))
            .SendAsync("ReceiveMessage", senderId, message, conversationId);
    }

    public async Task MarkAsDelivered(string senderId, int messageId)
    {
        var receiverId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _messageService.UpdateDeliveredMessage(messageId, receiverId);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't update message status " + result.ErrorMessage);
        }

        var senderConnectionId = _connectionManager.GetConnectionId(senderId);
        if (senderConnectionId != null)
        {
            await Clients
                .Client(senderConnectionId)
                .SendAsync("ReceiverDelivered", receiverId, messageId);
        }
    }

    public async Task MarkAsRead(string senderId)
    {
        var receiverId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _messageService.UpdateReadMessages(senderId, receiverId);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't update message status " + result.ErrorMessage);
        }

        var receiverConnectionId = _connectionManager.GetConnectionId(senderId);
        if (receiverConnectionId != null)
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiverRead", receiverId);
        }
    }

    public async Task StartedTyping(int conversationId)
    {
        var typerId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        await Clients.Group(GetGroupNameFromId(conversationId)).SendAsync("StartedTyping", typerId);
    }

    public async Task StoppedTyping(int conversationId)
    {
        var typerId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        await Clients.Group(GetGroupNameFromId(conversationId)).SendAsync("StoppedTyping", typerId);
    }

    public async Task JoinGroupChat(int groupId)
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _conversationService.AddUserToGroup(groupId, userId);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't join the group " + result.ErrorMessage);
        }

        await Clients
            .Group(GetGroupNameFromId(groupId))
            .SendAsync("JoinedGroupChat", userId, groupId);
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupNameFromId(groupId));
    }

    public async Task LeaveGroupChat(int groupId)
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _conversationService.RemoveUserFromGroup(groupId, userId);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't leave the group " + result.ErrorMessage);
        }

        await Clients
            .Group(GetGroupNameFromId(groupId))
            .SendAsync("LeftGroupChat", userId, groupId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupNameFromId(groupId));
    }

    public async Task ChangeNickname(int conversationId, string newNickname)
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _conversationService.ChangeNickname(conversationId, userId, newNickname);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't change the nickname " + result.ErrorMessage);
        }

        await Clients
            .Group(GetGroupNameFromId(conversationId))
            .SendAsync("ChangedNickname", userId, newNickname, conversationId);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        _connectionManager.AddConnection(userId, Context.ConnectionId);
        var result = await _conversationService.GetUserConversations(userId);
        if (!result.IsSuccess)
        {
            throw new HubException(result.ErrorMessage);
        }
        foreach (var group in result.Data!)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupNameFromId(group.Id));
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        _connectionManager.RemoveConnection(userId);

        var result = await _conversationService.GetUserConversations(userId);
        if (!result.IsSuccess)
        {
            throw new HubException(result.ErrorMessage);
        }

        foreach (var group in result.Data!)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupNameFromId(group.Id));
        }

        await base.OnDisconnectedAsync(exception);
    }

    private string GetGroupNameFromId(int conversationId)
    {
        return $"group_{conversationId}";
    }
}
