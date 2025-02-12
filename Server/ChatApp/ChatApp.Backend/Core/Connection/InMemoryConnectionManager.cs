using System.Collections.Concurrent;
using ChatApp.Backend.Core.Common;

namespace ChatApp.Backend.Core.Connection;

public class InMemoryConnectionManager : IConnectionManager
{
    private ConcurrentDictionary<string, string> _userConnections;

    public InMemoryConnectionManager(ConcurrentDictionary<string, string> userConnections)
    {
        _userConnections = new ConcurrentDictionary<string, string>();
    }

    public void AddConnection(string userId, string connectionId)
    {
        _userConnections.TryAdd(userId, connectionId);
    }

    public void RemoveConnection(string userId)
    {
        _userConnections.TryRemove(userId, out _);
    }

    public string? GetConnectionId(string userId)
    {
        _userConnections.TryGetValue(userId, out string? connectionId);
        return connectionId;
    }

    public List<string> GetConnectedUsers()
    {
        return _userConnections.Keys.ToList();
    }

    public List<string> GetConnectionIds()
    {
        return _userConnections.Values.ToList();
    }
}
