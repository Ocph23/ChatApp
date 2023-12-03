using ChatApp.Service;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared;
using Shared.Contracts;

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
            var sender = Context.User!.GetUserId();
            var connection = await chatUserManager.GetConnectionId(sender!);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendPrivateMessage(string reciveId, Message message)
        {
            var senderId = Context.User!.GetUserId();
            var targetConnection = await chatUserManager.GetConnectionId(reciveId);
            var newMessage = new MessagePrivate()
            {
                MessageType = message.MessageType,
                PenerimaId = reciveId,
                PengirimId = senderId,
                Text = message.Text,
                UrlFile = message.UrlFile,
                Tanggal = DateTime.Now.ToUniversalTime(),
            };
            var messageResult= await messageService.PostPrivateMessage(newMessage);
            if (!string.IsNullOrEmpty(targetConnection))
            {
                await Clients.Client(targetConnection).SendAsync("ReceivePrivateMessage", messageResult);
            }
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User!.GetUserId();
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
