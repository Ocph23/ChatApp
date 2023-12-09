using ChatAppMobile.Messages;
using ChatAppMobile.Services;
using ChatAppMobile.Validator;
using ChatAppMobile.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using OcphApiAuth.Client;
using Shared;

namespace ChatAppMobile.Pages;

public partial class AddContactPage : Popup
{
    private AddContactViewModel vm;

    public AddContactPage()
    {
        InitializeComponent();
        BindingContext = vm = new AddContactViewModel();
        vm.OnCloseAddContact += Vm_OnCloseAddContact;
    }

    private void Vm_OnCloseAddContact(object? sender, AddContactViewModel e)
    {
        this.Close();
    }
}


public class AddContactViewModel : BaseViewModel
{

    public event EventHandler<AddContactViewModel> OnCloseAddContact;

    AddContactValidator validator = new AddContactValidator();

    public AddContactViewModel()
    {
        AddCommand = new Command(async (x) => await AddCommandAction(x), AddCommandValidation);
        CancelCommand = new Command(() => OnCloseAddContact?.Invoke(null, this));
        this.PropertyChanged += (s, p) =>
        {
            if (p.PropertyName == "Email")
            {
                AddCommand.ChangeCanExecute();
            }
        };
    }


    public Command CancelCommand { get; set; }


    public Command addCommand;

    public Command AddCommand
    {
        get { return addCommand; }
        set { SetProperty(ref addCommand, value); }
    }


    private string email;

    public string Email
    {
        get { return email; }
        set { SetProperty(ref email, value); }
    }



    public async Task AddCommandAction(object obj)
    {
        try
        {
            var contactService = ServiceHelper.GetService<IContactService>();
            var authStateProvider = ServiceHelper.GetService<OcphAuthStateProvider>();
            var userid = await authStateProvider.GetUserId();
            if (userid != null)
            {
                var result = await contactService.AddTemanByUserName( Email);
                if (result !=null)
                {
                    WeakReferenceMessenger.Default.Send(new AddContactMessageChange(result));
                    OnCloseAddContact?.Invoke(result, this);
                }
            }
        }
        catch (Exception ex)
        {
            await AppHelper.ShowMessage(ex.Message);
        }
    }

    public bool AddCommandValidation(object obj)
    {
        var validate = validator.Validate(this);
        return validate.IsValid;
    }

}