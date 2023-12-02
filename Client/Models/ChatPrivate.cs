using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Client.Models
{
    public class ChatPrivate : IDisposable
    {
        public event EventHandler OnReciveMessage; 
        public User? myAccount { get; set; }
        private HubConnection hubConnection;
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public User? otherAccount { get; set; }
        public ChatPrivate(User _myAccount, string _myAccessToken)
        {
            myAccount = _myAccount;
            _ = Connect(_myAccessToken);
        }
        private async Task Connect(string token)
        {

            hubConnection = new HubConnectionBuilder()
       .WithUrl($"{Helper.ClientURL}/chathub", options =>
       {
           options.AccessTokenProvider = () => Task.FromResult($"{token}");
           options.Headers.Add("Authorization", $"{token}");
       }).Build();

            hubConnection.On<Message>("ReceiveMessage", (message) =>
            {
                var encodedMsg = $"{message.MessageType}: {message.Text}";
                Messages.Add(message);
                OnReciveMessage?.Invoke(message, new EventArgs());
            });

            await hubConnection.StartAsync();
        }

        public async Task Send(string message)
        {
            if (hubConnection is not null && IsConnected)
            {
                await hubConnection.SendAsync("SendMessage",  new Message() { Text=message});
            }
        }

        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public async void Dispose()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
