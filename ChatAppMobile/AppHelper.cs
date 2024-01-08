using ChatAppMobile.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile
{
    public static class AppHelper
    {
        public static async Task AppDisplayError(string message)
        {
           await Application.Current.MainPage.DisplayAlert("Error",message,"Keluar");
        }

        public static async Task ShellDisplayError(string message)
        {
            await Shell.Current.DisplayAlert("Error", message, "Keluar");
        }

        internal static void Login()
        {
            Application.Current.MainPage = new LoginPage();
        }

        internal static async Task ShowMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Info", message, "OK");
        }
    }
}
