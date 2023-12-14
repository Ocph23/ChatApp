using ChatAppMobile.Models;

namespace ChatAppMobile
{
    public partial class AppShell : Shell
    {
        private ChatClient chatClient;
        public AppShell()
        {
            InitializeComponent();

            InitializeComponent();
            chatClient = ServiceHelper.GetService<ChatClient>();
            _=chatClient.Start();
        }

    }
}
