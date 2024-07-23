using ChatAppMobile.Messages;
using ChatAppMobile.Models;
using ChatAppMobile.Services;
using ChatAppMobile.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Shared;

namespace ChatAppMobile.Pages;

public partial class ChatPage : ContentPage
{
    public ChatPage()
    {
        InitializeComponent();
        BindingContext = new ChatViewModel();
    }



    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SearchPage());
    }
}


public class ChatViewModel : BaseViewModel
{

    private MobileContact chatContact;
    private bool showSelectItem;
    public MobileContact ChatContact
    {
        get { return chatContact; }
        set { SetProperty(ref chatContact, value); }
    }

    public bool ShowSelectItem
    {
        get { return showSelectItem; }
        set { SetProperty(ref showSelectItem, value); }
    }


    public Command AddCommandContact { get; set; }
    public Command CreateGroupCommand { get; }
    public Command LoadCommand { get; }
    public Command SelectCommand { get; set; }


    private Command showItemCommand;

    public Command ShowItemCommand
    {
        get { return showItemCommand; }
        set { SetProperty(ref showItemCommand, value); }
    }



    public ChatViewModel()
    {
        LoadCommand = new Command(async (x) => await LoadCommandAction(x));
        SelectCommand = new Command(SelectCommandAction);
        ShowItemCommand = new Command(() => ShowSelectItem = !ShowSelectItem);
        AddCommandContact = new Command(AddCommandContactAction);
        CreateGroupCommand = new Command(async (obj) => await CreateGroupAction(obj));
        WeakReferenceMessenger.Default.Register<AddContactMessageChange>(this, (r, m) =>
        {
            if (m != null && m.Value != null)
            {
                ChatContact.Friends.Add(m.Value);

            }
        });


        WeakReferenceMessenger.Default.Register<PrivateMessageChange>(this, (r, m) =>
        {
            if (m != null && m.Value != null)
            {
                var teman = ChatContact.Friends.FirstOrDefault(x => x.TemanId == m.Value.PengirimId);
                teman.AddMessage(m.Value);
                //teman.Messages.Add(m.Value);
            }
        });

        LoadCommand.Execute(null);

    }

    private async Task LoadCommandAction(object obj)
    {
        try
        {
            if (IsBusy) return;
            IsBusy = true;
            var service = ServiceHelper.GetService<IContactService>();
            ChatContact = await service.Get();
        }
        catch (Exception ex)
        {
            await AppHelper.ShowMessage(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CreateGroupAction(object obj)
    {
        var members = ChatContact.Friends.Where(x => x.Selected);
        if (!members.Any())
        {
            await AppHelper.ShowMessage("Anda Harus Memilih Minimal 1 Anggota group");
        }
        else
        {
            Shell.Current.ShowPopup(new AddGroupPage(members));
        }
    }

    private void AddCommandContactAction(object obj)
    {
        Shell.Current.ShowPopup(new AddContactPage());
    }

    private void SelectCommandAction(object obj)
    {
        var teman = obj as TemanViewModel;
        Shell.Current.Navigation.PushAsync(new Pages.ChatPrivatreRoom(teman));
    }

}