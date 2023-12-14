
namespace ChatAppMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new AppShell();
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
