using ChatApp.Service;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared;

namespace ChatApp.SignalRApp
{

    public class ChatHub : Hub
    {
        private readonly IChatUserManager chatUserManager;
        private readonly IMessageService messageService;

        public ChatHub(IChatUserManager chatUserManager, IMessageService messageService)
        {
            this.chatUserManager = chatUserManager;
            this.messageService = messageService;
        }
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendPrivateMessage(string userId, string connectionId, Message message)
        {
            await Clients.All.SendAsync("ReceivePrivateMessage", message);
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.GetUserId();
            chatUserManager.AddConnection(userId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            chatUserManager.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
