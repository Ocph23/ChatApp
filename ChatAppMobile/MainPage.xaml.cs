using ChatAppMobile.Models;
using ChatAppMobile.Validator;
using ChatAppMobile.ViewModels;
using OcphApiAuth.Client;

namespace ChatAppMobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private ChatClient chatClient;

        public MainPage()
        {
            InitializeComponent();
            chatClient = ServiceHelper.GetService<ChatClient>();
            _= Load();
        }

        private async Task Load()
        {
          await  chatClient.Start();
        }
    }
}
