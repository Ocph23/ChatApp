using ChatAppMobile.Validator;
using ChatAppMobile.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using ChatAppMobile.Services;
using ChatAppMobile.Messages;
using OcphApiAuth.Client;
using Shared;

namespace ChatAppMobile.Pages;

public partial class AddGroupMember : Popup
{
    private AddGroupMemberViewModel viewmodel;
    public AddGroupMember(GroupDTO group)
    {
        InitializeComponent();
        BindingContext = viewmodel = new AddGroupMemberViewModel(group);
        viewmodel.OnCloseAddMember += Viewmodel_OnCloseAddMember; ;
    }

    private void Viewmodel_OnCloseAddMember(object? sender, AddGroupMemberViewModel e)
    {
       this.Close(sender);
    }
}


public class AddGroupMemberViewModel : BaseViewModel
{
    public event EventHandler<AddGroupMemberViewModel> OnCloseAddMember;

    AddMemberValidator validator = new AddMemberValidator();
    GroupDTO Group { get; set; }

    public AddGroupMemberViewModel(GroupDTO group)
    {
        Group = group;
        AddCommand = new Command(async (x) => await AddCommandAction(x), AddCommandValidation);
        CancelCommand = new Command(() => OnCloseAddMember?.Invoke(null, this));
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
                var result = await contactService.AddAnggota(Group.Id,Email);
                if (result != null)
                {
                    WeakReferenceMessenger.Default.Send(new AddMemberMessageChange(Group));
                    OnCloseAddMember?.Invoke(result, this);
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