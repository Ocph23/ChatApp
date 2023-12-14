using Microsoft.AspNetCore.SignalR.Client;
using Shared.Contracts;
using System;
using ChatAppMobile.Services;

using CommunityToolkit.Mvvm.Messaging;
using ChatAppMobile.Messages;
using Shared;



namespace ChatAppMobile.Models
{
    public class ChatClient : IDisposable
    {
        private readonly IMessageService messageService;
        private readonly IContactService contactService;

        string auth_token;
        // public event EventHandler OnReciveMessage;
        private HubConnection hubConnection;
        public MobileContact Contact { get; set; }
        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;
        public ChatClient(IMessageService messageService, IContactService contactService)
        {
            this.messageService = messageService;
            this.contactService = contactService;

            WeakReferenceMessenger.Default.Register<PrivateSendMessageChange>(this, (r, m) =>
            {
                if (m != null && m.Value != null)
                {
                    SendPrivateMessage(m.Value);
                }
            });


            WeakReferenceMessenger.Default.Register<GroupSendMessageChange>(this, (r, m) =>
            {
                if (m != null && m.Value != null)
                {
                    SendGroup(m.Value);
                }
            });


        }

        public async Task Start()
        {
            auth_token = Preferences.Get("token", string.Empty);

            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                           .WithUrl($"{Helper.ServerURL}/chathub", options =>
                           {
                               options.AccessTokenProvider = () => Task.FromResult($"{auth_token}");
                               options.Headers.Add("Authorization", $"{auth_token}");
                           }).WithAutomaticReconnect()
                           .Build();



                hubConnection.On<MessageGroup>("ReceiveGroupMessage", (message) =>
                {
                    var encodedMsg = $"{message.MessageType}: {message.Text}";
                    //  Messages.Add(message);
                    // OnReciveMessage?.Invoke(message, new EventArgs());
                });


                hubConnection.On<MessagePrivate?>("ReceivePrivateMessage", (message) =>
                {
                    WeakReferenceMessenger.Default.Send(new PrivateMessageChange(message));
                });

                await hubConnection.StartAsync();
            }


            if (Contact == null)
            {
                var contact = await contactService.Get();
                Contact = new MobileContact();
                if (contact != null)
                {
                    foreach (var item in contact.Friends)
                    {
                        Contact.Friends.Add(item);
                    }
                }
            }

        }

        internal Task SendPrivateMessage(MessagePrivate message)
        {
            if (hubConnection is not null && IsConnected)
            {
                _ = hubConnection.SendAsync("SendPrivateMessage", message.PenerimaId, message);
            }
            return Task.CompletedTask;
        }

        public async Task SendGroup(MessageGroup message)  ///group
        {
            if (hubConnection is not null && IsConnected)
            {
                var model = new MessageGroup() { Text = message.Text, GroupId = message.GroupId, PengirimId = Contact.UserId, MessageType = Shared.MessageType.Text };
                await hubConnection.SendAsync("SendGroupMessage", model);
            }
        }

        //   public ChatRoom CurrentRoom { get; set; }

        public async Task SetCurrentChat(string userId)
        {
            var contact = Contact.Friends.FirstOrDefault(x => x.TemanId == userId);
            if (!contact.Messages.Any())
            {
                var messages = await messageService.GetPrivateMessage(Contact.UserId, contact.TemanId);
                foreach (var item in messages)
                {
                    contact.Messages.Add(item);
                }
            }

            // CurrentRoom = new ChatRoom(contact);
        }

        public async void Dispose()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

    }
}
