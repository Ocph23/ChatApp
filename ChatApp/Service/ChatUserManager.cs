namespace ChatApp.Service
{
    public interface IChatUserManager
    {
        Task AddConnection(string? userId, string? connectionId);
        Task<string?> GetConnectionId(string userId);
        Task Remove(string userid);
    }


    public class ChatUserManager : IChatUserManager
    {
        Dictionary<string, string> listConnection = new Dictionary<string, string>();
        public Task AddConnection(string? userId, string? connectionId)
        {
            lock (listConnection)
            {
                if (!listConnection.TryGetValue(userId, out var connection))
                {
                    listConnection.Add(userId, connectionId);
                }
                else
                {
                    listConnection[userId] = connectionId;
                }

            }
            return Task.CompletedTask;
        }

        public Task<string?> GetConnectionId(string userId)
        {
            if (listConnection.TryGetValue(userId, out var connectionid))
            {
                return Task.FromResult(connectionid)!;
            }
            return Task.FromResult(string.Empty)!;
        }

        public Task Remove(string userId)
        {
            lock (listConnection)
            {
                var result = listConnection.FirstOrDefault(x => x.Key == userId);
                if(listConnection.TryGetValue((string)userId, out var connectionid))
                {
                    listConnection.Remove(userId);
                }
                return Task.CompletedTask;
            }
        }
    }
}
