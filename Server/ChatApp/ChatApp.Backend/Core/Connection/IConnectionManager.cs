namespace ChatApp.Backend.Core.Connection;

public interface IConnectionManager
{
    public void AddConnection(string userId, string connectionId);

    public void RemoveConnection(string userId);

    public string? GetConnectionId(string userId);

    public List<string> GetConnectedUsers();
    public List<string> GetConnectionIds();
}
