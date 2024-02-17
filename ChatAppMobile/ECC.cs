using Elliptic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile
{
    public class ECC
    {
        public static byte[] GetPublicKey()
        {
            try
            {
                if (HasKey)
                {
                    var privateKey = GetPrivateKey();
                    return Curve25519.GetPublicKey(privateKey);
                }
                throw new SystemException();
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static bool HasKey => GetPrivateKey().Length <= 0 ? false : true;

        private static byte[] GetPrivateKey()
        {
            var key = Preferences.Get("privateKey", string.Empty);
            if (key != string.Empty)
            {
                return Convert.FromBase64String(key);
            }
            return GeneratePrivateKey();
        }

        public static byte[] GeneratePrivateKey()
        {
            try
            {
                byte[] senderRandomBytes = new byte[32];
                RNGCryptoServiceProvider.Create().GetBytes(senderRandomBytes);
                byte[] senderPrivate = Curve25519.ClampPrivateKey(senderRandomBytes);
                Preferences.Set("privateKey",Convert.ToBase64String(senderPrivate));
                return senderPrivate;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static string GetSharderKey(byte[] anyPublicKey)
        {
            var sharderKey = Curve25519.GetSharedSecret(GetPrivateKey(), anyPublicKey);
            return Convert.ToBase64String(sharderKey);
        }
    }
}
