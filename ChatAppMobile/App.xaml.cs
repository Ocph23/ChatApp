
namespace ChatAppMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Preferences.Remove("token");
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
