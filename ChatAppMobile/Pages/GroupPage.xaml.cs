using ChatAppMobile.Messages;
using ChatAppMobile.Models;
using ChatAppMobile.Services;
using ChatAppMobile.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Shared;


namespace ChatAppMobile.Pages;

public partial class GroupPage : ContentPage
{
    public GroupPage()
    {
        InitializeComponent();
        BindingContext = new GroupViewModel();
    }



    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new SearchPage());
    }
}


public class GroupViewModel : BaseViewModel
{

    private MobileContact chatContact;

    public MobileContact ChatContact
    {
        get { return chatContact; }
        set { SetProperty(ref chatContact, value); }
    }


    public Command AddCommandGroup { get; set; }

    public Command SelectCommand { get; set; }

    public GroupViewModel()
    {
        SelectCommand = new Command(SelectCommandAction);
        AddCommandGroup = new Command(AddCommandGroupAction);
        WeakReferenceMessenger.Default.Register<AddGroupMessageChange>(this, (r, m) =>
        {
            if (m != null && m.Value != null)
            {
                ChatContact.Groups.Add(m.Value);
            }
        });


        WeakReferenceMessenger.Default.Register<GroupMessageChange>(this, (r, m) =>
        {
            if (m != null && m.Value != null)
            {
                var group = ChatContact.Groups.FirstOrDefault(x => x.Id== m.Value.Id);
                group.AddMessage(m.Value);
            }
        });
        _ = Load();
    }

    private void AddCommandGroupAction(object obj)
    {
        Shell.Current.ShowPopup(new AddGroupPage());
    }

    private void SelectCommandAction(object obj)
    {
        var group = obj as GroupDTO;
        Shell.Current.Navigation.PushModalAsync(new Pages.ChatGroupRoom(group));
    }

    private async Task Load()
    {
        try
        {
            var service = ServiceHelper.GetService<IContactService>();
            ChatContact = await service.Get();

        }
        catch (Exception)
        {
            throw;
        }
    }
}