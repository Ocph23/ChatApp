using ChatAppMobile.Messages;
using ChatAppMobile.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using OcphApiAuth.Client;
using Shared;
using Shared.Contracts;
using System.Collections.ObjectModel;

namespace ChatAppMobile.Pages;

public partial class ChatGroupRoom : ContentPage
{
    private ChatGroupRoomViewModel viewModels;

    public ChatGroupRoom(GroupDTO? group)
    {
        InitializeComponent();
        BindingContext = viewModels = new ChatGroupRoomViewModel(group);
        viewModels.OnAddItem += ViewModels_OnAddItem;
    }

    private void ViewModels_OnAddItem(object? sender, EventArgs e)
    {
        var message = sender as MessageGroup;
        list.ScrollTo(message, ScrollToPosition.End);
    }
}

public class ChatGroupRoomViewModel : BaseViewModel
{

    public string MyId { get; set; }

    public event EventHandler OnAddItem;

    public ObservableCollection<MessageGroup> Messages { get; set; } = new ObservableCollection<MessageGroup>();


    public GroupDTO? Group { get; set; }

    private Command sendCommand;
    //private ChatClient chatClient;

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

    public ChatGroupRoomViewModel(GroupDTO? group)
    {

        WeakReferenceMessenger.Default.Register<GroupMessageChange>(this, async (r, m) =>
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

        FileCommand = new Command(FileCommandAction);
        BackCommand = new Command(BackCommandAction);
        SendCommand = new Command(async (x) => await SendCommandAction(x), SendCommandValidate);
        Group = group;
        Title = group.NameGroup;
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

    private async Task LoadMessage()
    {
        var authProviderService = ServiceHelper.GetService<OcphAuthStateProvider>();
        MyId = await authProviderService.GetUserId();
        var service = ServiceHelper.GetService<IMessageService>();
        var messgaes = await service.GetGroupMessage(Group.Id);
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
        //var readed = await service.ReadMassage(Teman.TemanId, MyId);
        //if (readed)
        //{
        //    foreach (var item in Teman.Messages)
        //    {
        //        item.Status = MessageStatus.Baca;
        //    }
        //}
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
        var message = new MessageGroup { PengirimId = MyId, IsMe = true, MessageType = MessageType.Text, Tanggal = DateTime.Now, Text = Message, GroupId=Group.Id};
        WeakReferenceMessenger.Default.Send(new GroupSendMessageChange(message));
        Messages.Add(message);
        OnAddItem?.Invoke(message, null);
        await Task.Delay(200);
        Message = string.Empty;
    }
}