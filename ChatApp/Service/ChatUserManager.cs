namespace ChatApp.Service
{
    public interface IChatUserManager
    {
        Task AddConnection(string? userId, string? connectionId);
        Task<UserConnection?> FindConnectionByUserId(string userId);
        Task<UserConnection?> FindConnectionByConnectionId(string connectionId);
        Task Remove(string connectionId);
    }


    public class ChatUserManager : IChatUserManager
    {
        List<UserConnection> listConnection = new List<UserConnection>();

        public Task AddConnection(string? userId, string? connectionId)
        {
            var oldConnection = listConnection.FirstOrDefault(x => x.UserId == userId);
            if (oldConnection != null)
                oldConnection.ConnectionId = connectionId;
            listConnection.Add(new UserConnection(userId, connectionId));
            return Task.CompletedTask;
        }

        public Task<UserConnection?> FindConnectionByConnectionId(string connectionId)
        {
            var result = listConnection.FirstOrDefault(x => x.UserId == connectionId);
            return Task.FromResult(result);
        }

        public Task<UserConnection?> FindConnectionByUserId(string userId)
        {
            var result = listConnection.FirstOrDefault(x => x.UserId == userId);
            return Task.FromResult(result);
        }

        public Task Remove(string connectionId)
        {
            var result = listConnection.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (result != null)
            {
                listConnection.Remove(result);
            }

            return Task.CompletedTask;
        }
    }
}
