using Blazored.LocalStorage;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Shared;
using Shared.Contracts;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Models
{
    public class ChatClient  :IDisposable
    {
        private readonly IMessageService messageService;
        private readonly ILocalStorageService localStorage;
        private readonly IContactService contactService;

        string auth_token;
        public event EventHandler OnReciveMessage;
        private HubConnection hubConnection;
        public Contact Contact { get; set; }
        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;
        public ChatClient(IMessageService messageService, ILocalStorageService localStorage, IContactService contactService)
        {
            this.messageService = messageService;
            this.localStorage = localStorage;
            this.contactService = contactService;
           
        }

        public async Task Start()
        {
            auth_token = await localStorage.GetItemAsync<string>("token");

            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                           .WithUrl($"{Helper.ServerURL}/chathub", options =>
                           {
                               options.AccessTokenProvider = () => Task.FromResult($"{auth_token}");
                               options.Headers.Add("Authorization", $"{auth_token}");
                           }).WithAutomaticReconnect()
                           .Build();



                hubConnection.On<Message>("ReceiveMessage", (message) =>
                {
                    var encodedMsg = $"{message.MessageType}: {message.Text}";
                  //  Messages.Add(message);
                    OnReciveMessage?.Invoke(message, new EventArgs());
                });


                hubConnection.On<MessagePrivate?>("ReceivePrivateMessage", (message) =>
                {
                    if (CurrentRoom is not null && CurrentRoom.IsPrivate)
                    {
                        var encodedMsg = $"{message.MessageType}: {message.Text}";
                        CurrentRoom.AddMessage(message);
                        OnReciveMessage?.Invoke(message, new EventArgs());
                    }
                    else
                    {
                        var teman = Contact.Friends.SingleOrDefault(x => x.TemanId == message.PengirimId);
                        if (teman != null)
                        {
                            teman.Messages.Add(message);
                        }
                    }
                });

                await hubConnection.StartAsync();
            }


            if (Contact == null)
            {
                Contact = await contactService.Get();
                if (Contact == null)
                {
                    Contact = new Contact();
                }
            }

        }
        public User? otherAccount { get; set; }
        public async Task Send(string messageText)
        {
            if (hubConnection is not null && IsConnected)
            {
                if (CurrentRoom != null && CurrentRoom.IsPrivate)
                {
                    var message = new MessagePrivate() { Text = messageText, PengirimId = Contact.UserId, MessageType = Shared.MessageType.Text };
                    _ = hubConnection.SendAsync("SendPrivateMessage", CurrentRoom.ReciveId, message);
                    _ = CurrentRoom.AddMessage(message);
                }
                else
                {
                    var message = new MessageGroup() { Text = messageText, PengirimId = Contact.UserId, MessageType = Shared.MessageType.Text };
                    await hubConnection.SendAsync("SendMessage", message);

                }
            }
        }

        public ChatRoom CurrentRoom { get; set; }

        public async Task SetCurrentChat(string userId)
        {
            var contact = Contact.Friends.FirstOrDefault(x => x.TemanId == userId);
            if (!contact.Messages.Any())
            {
                var messages = await messageService.GetPrivateMessage(Contact.UserId, contact.TemanId);
                contact.Messages = messages.ToList();
            }

            CurrentRoom = new ChatRoom(contact);
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
