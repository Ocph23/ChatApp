using System.Collections;

namespace ChatApp.Service
{

    public class UserConnection
    {
        public UserConnection(string? userId, string? connectionId)
        {
            UserId = userId;
            ConnectionId = connectionId;
        }

        public string? UserId { get; set; }
        public string? ConnectionId { get;  set; }
        public DateTime CreatedAt { get; private set;} =DateTime.Now;
        public DateTime? UpdateAt { get; private set; }=DateTime.Now;
    }
}
