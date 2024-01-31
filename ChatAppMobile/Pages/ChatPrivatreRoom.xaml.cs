using ChatAppMobile.Messages;
using ChatAppMobile.Models;
using ChatAppMobile.Services;
using ChatAppMobile.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using OcphApiAuth.Client;
using Shared;
using Shared.Contracts;
using System.Collections.ObjectModel;

namespace ChatAppMobile.Pages;

public partial class ChatPrivatreRoom : ContentPage
{
    private ChatPrivateRoomViewModel viewModels;

    public ChatPrivatreRoom(TemanViewModel? teman)
    {
        InitializeComponent();
        BindingContext = viewModels = new ChatPrivateRoomViewModel(teman);
        viewModels.OnAddItem += ViewModels_OnAddItem;
    }

    private void ViewModels_OnAddItem(object? sender, EventArgs e)
    {
        var message = sender as MessagePrivate;
        list.ScrollTo(message, ScrollToPosition.End);
    }   

    public class ChatPrivateRoomViewModel : BaseViewModel
    {

        public string MyId { get; set; }

        public event EventHandler OnAddItem;

        public ObservableCollection<MessagePrivate> Messages { get; set; } = new ObservableCollection<MessagePrivate>();


        public TemanViewModel? Teman { get; set; }

        private Command sendCommand;
        //private ChatClient chatClient;

        public Command BackCommand { get; set; }
        public Command FileCommand { get; set; }

        public Command SendCommand
        {
            get { return sendCommand; }
            set { SetProperty(ref sendCommand, value); }
        }

        public Command DownloadFileCommand { get; private set; }

        private string message;

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public ChatRoom CurrentRoom { get; private set; }

        public ChatPrivateRoomViewModel(TemanViewModel? teman)
        {

            WeakReferenceMessenger.Default.Register<PrivateMessageChange>(this, async (r, m) =>
            {
                try
                {
                    if (m != null && m.Value != null)
                    {
                        var data = m.Value;
                        data.IsMe = false;
                        if (!Messages.Any(x => x.Id == data.Id))
                        {
                            Messages.Add(data);
                            OnAddItem?.Invoke(data, new EventArgs());
                        }
                    }
                }
                catch (Exception)
                {
                }
            });

            FileCommand = new Command(async (x)=> await FileCommandAction(x));
            BackCommand = new Command(BackCommandAction);
            SendCommand = new Command(async (x) => await SendCommandAction(x), SendCommandValidate);
            DownloadFileCommand = new Command(async (x) => await DownloadFileCommandAction(x));
          



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

        private Task DownloadFileCommandAction(object x)
        {
            try
            {
                var fileService = ServiceHelper.GetService<IFileServices>();
                fileService.DownloadFile(x.ToString());
            }
            catch (Exception ex)
            {

                throw;
            }
            throw new NotImplementedException();
        }

        private async Task LoadMessage()
        {
            var authProviderService = ServiceHelper.GetService<OcphAuthStateProvider>();
            MyId = await authProviderService.GetUserId();
            CurrentRoom = new ChatRoom(Teman);
            var service = ServiceHelper.GetService<IMessageService>();
            var messgaes = await service.GetPrivateMessage(Teman.TemanId, MyId);
            foreach (var item in messgaes)
            {
                item.IsMe = item.PengirimId == MyId;
                Messages.Add(item);
                if (!item.IsMe)
                {
                    item.Status = MessageStatus.Baca;
                }
            }
            OnAddItem?.Invoke(Messages.Last(), null);
            var readed = await service.ReadMassage(Teman.TemanId, MyId);
            if (readed)
            {
                foreach (var item in Teman.Messages)
                {
                    item.Status = MessageStatus.Baca;
                }
            }
        }

        private async Task FileCommandAction(object obj)
        {

            string action = await Shell.Current.DisplayActionSheet("Media ?", "Cancel", null, "Galery", "Camera");
            if (!string.IsNullOrEmpty(action))
            {
                if (action == "Galery")
                {
                    try
                    {
                        var result = await FilePicker.Default.PickAsync(new PickOptions());
                        if (result != null)
                        {

                            // pdf, xlx, xlsx, doc, docx, jpg dan pptx p

                            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                                result.FileName.EndsWith("xls", StringComparison.OrdinalIgnoreCase) ||
                                result.FileName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase) ||
                                result.FileName.EndsWith("doc", StringComparison.OrdinalIgnoreCase) ||
                                result.FileName.EndsWith("docx", StringComparison.OrdinalIgnoreCase) ||
                               result.FileName.EndsWith("ppt", StringComparison.OrdinalIgnoreCase) ||
                                result.FileName.EndsWith("pptx", StringComparison.OrdinalIgnoreCase) ||
                                result.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                            {
                                using var stream = await result.OpenReadAsync();
                                using var memoryStream = new MemoryStream();
                                await stream.CopyToAsync(memoryStream);
                                var data = memoryStream.ToArray();
                                var image = ImageSource.FromStream(() => stream);

                                var content = new MultipartFormDataContent();
                                content.Add(new StreamContent(await result.OpenReadAsync()), "file", result.FileName);
                                IFileServices fileService = ServiceHelper.GetService<IFileServices>();
                                string fileName = await fileService.UploadPrivateFile(Teman.TemanId, content);
                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    var messageService = ServiceHelper.GetService<IMessageService>();
                                    var item = new MessagePrivate { Text=result.FileName, MessageType = MessageType.File, UrlFile = fileName, PenerimaId= Teman.TemanId, 
                                        Tanggal = DateTime.Now, PengirimId = MyId };
                                    item.IsMe = item.PengirimId == MyId;
                                    Messages.Add(item);
                                    if (!item.IsMe)
                                    {
                                        item.Status = MessageStatus.Baca;
                                    }
                                    var resultx = await messageService.PostPrivateMessage(item);
                                    OnAddItem?.Invoke(Messages.Last(), null);
                                    return;
                                }
                            }
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        // The user canceled or something went wrong
                    }
                    return;
                }



                if (action == "Camera")
                {


                    return;
                }
            }
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
            var message = new MessagePrivate { PengirimId = MyId, IsMe = true, MessageType = MessageType.Text, Tanggal = DateTime.Now, Text = Message, PenerimaId = Teman.TemanId };
            WeakReferenceMessenger.Default.Send(new PrivateSendMessageChange(message));
            Messages.Add(message);
            OnAddItem?.Invoke(message, null);
            await Task.Delay(200);
            Message = string.Empty;
        }
    }
}