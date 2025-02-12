using System.Security.Claims;
using ChatApp.Backend.Core.Connection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Backend.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IConnectionManager _connectionManager;

    public ChatHub(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task SendMessage() { }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        _connectionManager.AddConnection(userId, Context.ConnectionId);
        return Task.CompletedTask;
    }
}
