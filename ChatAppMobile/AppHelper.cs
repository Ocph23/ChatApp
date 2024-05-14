using ChatAppMobile.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        private static byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] Encrypt(byte[] data, string _key)
        {
            if (CanPerformCryptography(data))
            {
                using (var aes = new AesManaged())
                {

                    aes.Key = Convert.FromBase64String(_key);
                    aes.IV = Convert.FromBase64String("h9GeWnVeV2no4pkircgXAg==");
                    using (var encryptor = aes.CreateEncryptor())
                    {
                        return PerformCryptography(encryptor, data);
                    }

                }
            }
            return data;
        }


        //public static byte[] Encrypt(byte[] data, byte[] _key)
        //{
        //    if (CanPerformCryptography(data))
        //    {
        //        using (var aes = new AesManaged())
        //        {

        //            aes.Key = _key;
        //            aes.IV = null;
        //            using (var encryptor = aes.CreateEncryptor())
        //            {
        //                return PerformCryptography(encryptor, data);
        //            }

        //        }
        //    }
        //    return data;
        //}

        private static bool CanPerformCryptography(byte[] data)
        {
            if (data.Length == 0)
                return false;
            return true;
        }

        public static byte[] Decrypt(byte[] data, string _key)
        {
            if (CanPerformCryptography(data))
            {
                using (var aes = new AesManaged())
                {

                    aes.Key = Convert.FromBase64String(_key);
                    aes.IV = Convert.FromBase64String("h9GeWnVeV2no4pkircgXAg==");
                    using (var encryptor = aes.CreateDecryptor())
                    {
                        return PerformCryptography(encryptor, data);
                    }
                }
            }
            return data;
        }
    }
}
