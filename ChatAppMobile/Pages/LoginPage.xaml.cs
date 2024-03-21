using ChatAppMobile.Validator;
using ChatAppMobile.ViewModels;
using OcphApiAuth.Client;
using Shared;

namespace ChatAppMobile.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }

    public class LoginViewModel : BaseViewModel
    {
        public LoginValidator validator = new LoginValidator();
        public LoginViewModel()
        {
            LoginCommand = new Command(async (x) => await LoginCommandAction(x), LoginCommandValidate);
            RegisterCommand = new Command(RegisterCommandAction);
            this.PropertyChanged += (_, __) =>
            {
                if (__.PropertyName == "UserName" || __.PropertyName == "Password")
                {
                    LoginCommand.ChangeCanExecute();
                }
            };
        }

        private async void RegisterCommandAction(object obj)
        {
            Application.Current.MainPage = new Pages.RegisterPage();
        }

        private async Task LoginCommandAction(object obj)
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                var accountService = ServiceHelper.GetService<IAccountService>();
                var result = await accountService.Login(new Client.OcphAuthClient.Models.LoginRequest { UserName = UserName, Password = Password });
                if (result != null)
                {
                    Preferences.Set("token", result.Token);
                    Preferences.Set("email", result.Email);
                    Preferences.Set("userName", result.UserName);
                    Preferences.Set("privateKey", result.PrivateKey);
                    Application.Current.MainPage = new AppShell();
                    return;
                }
                throw new UnauthorizedAccessException("UserName atau Password Anda Salah");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Keluar");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool LoginCommandValidate(object arg)
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

        private string? userName ;

        public string? UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        private string? password ;

        public string? Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string url = Helper.ServerURL;
        public string URL
        {
            get { return  url; }
            set { SetProperty(ref url , value); }
        }

    }
}