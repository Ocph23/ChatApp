using ChatAppMobile.Validator;
using ChatAppMobile.ViewModels;
using OcphApiAuth.Client;

namespace ChatAppMobile.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterViewModel();
    }
}

public class RegisterViewModel : BaseViewModel
{
                        
    RegisterValidator validator = new RegisterValidator();
    public RegisterViewModel()
    {
        LoginCommand = new Command(LoginCommandAction);
        RegisterCommand = new Command(RegisterCommandAction, RegisterCommandValidate);

        this.PropertyChanged += (_, __) =>
        {
            if (__.PropertyName == "UserName" ||
            __.PropertyName == "Name" ||
            __.PropertyName == "Password" ||
            __.PropertyName == "ConfirmPassword" ||
            __.PropertyName == "Telepon")
            {
                RegisterCommand.ChangeCanExecute();
            }
        };
    }

    private async void RegisterCommandAction(object obj)
    {
        try
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var service = ServiceHelper.GetService<IAccountService>();
            var result = await service.Register(new Client.OcphAuthClient.Models.RegisterRequest(UserName, Telepon, Password, ConfirmPassword, "User"));
            if (result != null)
            {
                await AppHelper.ShowMessage("Berhasil !");
            }
        }
        catch (Exception ex)
        {
            await AppHelper.AppDisplayError(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LoginCommandAction(object obj)
    {
        Application.Current.MainPage = new Pages.LoginPage();
    }

    private bool RegisterCommandValidate(object arg)
    {
        var validateResult = validator.Validate(this);
        Error = !validateResult.IsValid ? validateResult.Errors.First().ErrorMessage : string.Empty;
        return validateResult.IsValid;
    }

    private Command? loginCommand;

    public Command? LoginCommand
    {
        get { return loginCommand; }
        set { SetProperty(ref loginCommand, value); }
    }


    private Command? registerCommand;

    public Command? RegisterCommand
    {
        get { return registerCommand; }
        set { SetProperty(ref registerCommand, value); }
    }




    private string? userName;

    public string? UserName
    {
        get { return userName; }
        set { SetProperty(ref userName, value); }
    }


    private string? password;

    public string? Password
    {
        get { return password; }
        set { SetProperty(ref password, value); }
    }


    private string? confirmPassword;

    public string? ConfirmPassword
    {
        get { return confirmPassword; }
        set { SetProperty(ref confirmPassword, value); }
    }


    private string? telepon;

    public string? Telepon
    {
        get { return telepon; }
        set { SetProperty(ref telepon, value); }
    }



    private string name;

    public string Name
    {
        get { return name; }
        set { SetProperty(ref name, value); }
    }






}