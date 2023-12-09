using ChatAppMobile.Models;
using ChatAppMobile.ViewModels;
using Shared;
using Shared.Contracts;
using System.Collections.ObjectModel;

namespace ChatAppMobile.Pages;

public partial class ChatPrivatreRoom : ContentPage
{
    private ChatPrivateRoomViewModel viewModels;

    public ChatPrivatreRoom(Shared.TemanDTO? teman)
    {
        InitializeComponent();
        BindingContext = viewModels = new ChatPrivateRoomViewModel(teman);
        viewModels.OnAddItem += ViewModels_OnAddItem;
    }

    private void ViewModels_OnAddItem(object? sender, EventArgs e)
    {
        ChatMessage? chat = sender as ChatMessage;
        if (chat != null)
        {
            list.ScrollTo(chat, ScrollToPosition.End);
        }
    }

    public class ChatPrivateRoomViewModel : BaseViewModel
    {

        public event EventHandler OnAddItem;

        public ObservableCollection<ChatMessage> Messages { get; set; } = new ObservableCollection<ChatMessage>();


        public TemanDTO? Teman { get; set; }

        private Command sendCommand;
        private ChatClient chatClient;

        public Command BackCommand { get; set; }
        public Command FileCommand { get; set; }

        public Command SendCommand
        {
            get { return sendCommand; }
            set { SetProperty(ref sendCommand, value); }
        }



        private string message;

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }


        public ChatPrivateRoomViewModel(TemanDTO? teman)
        {

            chatClient = ServiceHelper.GetService<ChatClient>();

            FileCommand = new Command(FileCommandAction);
            BackCommand = new Command(BackCommandAction);
            SendCommand = new Command(async (x) => await SendCommandAction(x), SendCommandValidate);
            Title = teman.Nama;
            Teman = teman;
            this.PropertyChanged += (s, p) =>
            {
                if (p.PropertyName == "Message")
                {
                    SendCommand.ChangeCanExecute();
                }
            };

            _ = LoadMessage();
        }

        private void FileCommandAction(object obj)
        {
            
            throw new NotImplementedException();
        }

        private void BackCommandAction(object obj)
        {
            Shell.Current.Navigation.PopModalAsync();
        }

        private bool SendCommandValidate(object arg)
        {
            if (!string.IsNullOrEmpty(Message))
                return true;
            return false;
        }

        private async Task SendCommandAction(object obj)
        {
            var m = new ChatMessage(Message, DateTime.Now, true);
            Messages.Add(m);
            OnAddItem?.Invoke(m, null);
            await Task.Delay(200);
            Message = string.Empty;
        }

        private async Task LoadMessage()
        {
            var service = ServiceHelper.GetService<IMessageService>();
            var messgae = await service.GetPrivateMessage("1", "2");
            foreach (var item in messgae)
            {
                Messages.Add(new ChatMessage(item.Text, item.Tanggal, item.PengirimId == Teman.TemanId ? true : false));
            }
        }
    }
}