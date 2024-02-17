

using Elliptic;
using System.Security.Cryptography;

namespace ChatAppMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new AppShell();

            //Preferences.Set("privateKey", string.Empty);


            //var xxx = ECC.GetPublicKey();
            //var xxxxxx = ECC.GetSharderKey(xxx);

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

        private void Test()
        {
            // what Alice does
            byte[] senderRandomBytes = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(senderRandomBytes);

            byte[] senderPrivate = Curve25519.ClampPrivateKey(senderRandomBytes);
            byte[] senderPublic = Curve25519.GetPublicKey(senderPrivate);

            // what Bob does
            byte[] reciveRandomBytes = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(reciveRandomBytes);

            byte[] recivePrivate = Curve25519.ClampPrivateKey(reciveRandomBytes);
            byte[] recivePublic = Curve25519.GetPublicKey(recivePrivate);

            // what Alice does with Bob's public key
            byte[] senderShared = Curve25519.GetSharedSecret(senderPrivate, recivePublic);

            // what Bob does with Alice' public key
            byte[] reciveShared = Curve25519.GetSharedSecret(recivePrivate, senderPublic);

            // senderShared == reciveShared
        }
    }
}
