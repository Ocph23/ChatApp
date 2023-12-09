
namespace ChatAppMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new AppShell();
            //Preferences.Remove("user");
            string? token = Preferences.Get("token", string.Empty);
            if (string.IsNullOrEmpty(token))
            {
                MainPage = new Pages.LoginPage();
            }
            else
            {
                MainPage = new AppShell();
            }

        }

    }
}
