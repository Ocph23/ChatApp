using ChatAppMobile.Messages;
using ChatAppMobile.Validator;
using ChatAppMobile.ViewModels;
using ChatAppMobile.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using OcphApiAuth.Client;
using ChatAppMobile.Models;

namespace ChatAppMobile.Pages;

public partial class AddGroupPage : Popup
{
    private AddGroupViewModel vm;

    public AddGroupPage(IEnumerable<Models.TemanViewModel> member)
    {
        InitializeComponent();
        BindingContext = vm = new AddGroupViewModel(member);
        vm.OnCloseAddContact += Vm_OnCloseAddContact1; ;
    }

    private void Vm_OnCloseAddContact1(object? sender, AddGroupViewModel e)
    {
        this.Close();
    }

}



public class AddGroupViewModel : BaseViewModel
{

    public event EventHandler<AddGroupViewModel> OnCloseAddContact;

    AddGroupValidator validator = new AddGroupValidator();

    public AddGroupViewModel(IEnumerable<Models.TemanViewModel> member)
    {
        Members = member;
        AddCommand = new Command(async (x) => await AddCommandAction(x), AddCommandValidation);
        CancelCommand = new Command(() => OnCloseAddContact?.Invoke(null, this));
        this.PropertyChanged += (s, p) =>
        {
            if (p.PropertyName == "GroupName" || p.PropertyName == "Description")
            {
                AddCommand.ChangeCanExecute();
            }
        };
    }


    public Command CancelCommand { get; set; }


    public Command addCommand;

    public IEnumerable<TemanViewModel> Members { get; }

    public Command AddCommand
    {
        get { return addCommand; }
        set { SetProperty(ref addCommand, value); }
    }

    private string groupName;

    public string GroupName
    {
        get { return groupName; }
        set { SetProperty(ref groupName, value); }
    }


    private string description;

    public string Description
    {
        get { return description; }
        set { SetProperty(ref description, value); }
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
                var result = await contactService.CreateGroup(userid, new Shared.GroupDTO() { NameGroup = GroupName, Description = Description, 
                    Anggota = Members.Select(x=> new Shared.TemanDTO { TemanId= x.TemanId, Keanggotaan= Shared.KeanggotaanGroup.Anggota, Nama=x.Nama }) });
                if (result != null)
                {
                    WeakReferenceMessenger.Default.Send(new AddGroupMessageChange(result));
                    OnCloseAddContact?.Invoke(result, this);
                    await AppHelper.ShowMessage("Group nerhasil dibuat ");
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