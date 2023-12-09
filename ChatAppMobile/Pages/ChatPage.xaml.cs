using ChatAppMobile.Messages;
using ChatAppMobile.Services;
using ChatAppMobile.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Shared;
using Shared.Contracts;
using Contact = Shared.Contact;

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

    private Contact chatContact;

    public Contact ChatContact
    {
        get { return chatContact; }
        set { SetProperty(ref chatContact, value); }
    }


    public Command AddCommandContact { get; set; }

    public Command SelectCommand { get; set; }

    public ChatViewModel()
    {
        SelectCommand = new Command(SelectCommandAction);
        AddCommandContact = new Command(AddCommandContactAction);
        WeakReferenceMessenger.Default.Register<AddContactMessageChange>(this, (r, m) =>
        {
            if(m!=null && m.Value != null)
            {
                chatContact.Friends.Add(m.Value);
            }
        });
        _ = Load();
    }

    private void AddCommandContactAction(object obj)
    {
       Shell.Current.ShowPopup(new AddContactPage());
    }

    private void SelectCommandAction(object obj)
    {
        var teman = obj as TemanDTO;
        Shell.Current.Navigation.PushModalAsync(new Pages.ChatPrivatreRoom(teman));
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