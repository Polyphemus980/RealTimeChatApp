using System.Security.Claims;
using ChatApp.Backend.Core.Connection;
using ChatApp.Backend.Core.Groups;
using ChatApp.Backend.Core.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Backend.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IConnectionManager _connectionManager;
    private readonly IMessageService _messageService;
    private readonly GroupService _groupService;

    public ChatHub(
        IConnectionManager connectionManager,
        IMessageService messageService,
        GroupService groupService
    )
    {
        _connectionManager = connectionManager;
        _messageService = messageService;
        _groupService = groupService;
    }

    public async Task SendMessage(string receiverId, string message)
    {
        var senderId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _messageService.CreateMessageAsync(senderId, receiverId, message);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't send a message " + result.ErrorMessage);
        }

        var receiverConnectionId = _connectionManager.GetConnectionId(receiverId);
        if (receiverConnectionId != null)
        {
            await Clients
                .Client(receiverConnectionId)
                .SendAsync("ReceiveMessage", senderId, message);
        }
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

    public async Task StartedTyping(string receiverId)
    {
        var typerId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var receiverConnectionId = _connectionManager.GetConnectionId(receiverId);
        if (receiverConnectionId != null)
        {
            await Clients.Client(receiverConnectionId).SendAsync("StartedTyping", typerId);
        }
    }

    public async Task StoppedTyping(string receiverId)
    {
        var typerId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var receiverConnectionId = _connectionManager.GetConnectionId(receiverId);
        if (receiverConnectionId != null)
        {
            await Clients.Client(receiverConnectionId).SendAsync("StoppedTyping", typerId);
        }
    }

    public async Task JoinGroupChat(int groupId)
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _groupService.AddUserToGroup(groupId, userId);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't join the group " + result.ErrorMessage);
        }

        await Clients.Group(GetGroupNameFromId(groupId)).SendAsync("JoinedGroupChat", userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupNameFromId(groupId));
    }

    public async Task LeaveGroupChat(int groupId)
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _groupService.RemoveUserFromGroup(groupId, userId);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't leave the group " + result.ErrorMessage);
        }

        await Clients.Group(GetGroupNameFromId(groupId)).SendAsync("LeftGroupChat", userId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupNameFromId(groupId));
    }

    public async Task ChangeNickname(int groupId, string newNickname)
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _groupService.ChangeNickname(groupId, userId, newNickname);
        if (!result.IsSuccess)
        {
            throw new HubException("Couldn't change the nickname " + result.ErrorMessage);
        }

        await Clients.Group(GetGroupNameFromId(groupId)).SendAsync("ChangedNickname", userId);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        _connectionManager.AddConnection(userId, Context.ConnectionId);
        var result = await _groupService.GetUserGroups(userId);
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

        var result = await _groupService.GetUserGroups(userId);
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

    private string GetGroupNameFromId(int groupId)
    {
        return $"group_{groupId}";
    }
}
