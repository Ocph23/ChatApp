using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Shared
{
    public class Helper
    {

        public static string ServerURL => "https://w1vstpff-7148.asse.devtunnels.ms";
        //  public static string ServerURL => "https://localhost:7148";
        public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static string TitleCaseWithSpace(string p) => Regex.Replace(p, @"(?<=[a-z])([A-Z])", @" $1");


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

        public static byte[] Encrypt(byte[] data,string _key)
        {
            if (CanPerformCryptography(data))
            {
                using (var aes = new AesManaged())
                {

                    aes.Key = Convert.FromBase64String(_key);
                    aes.IV= Convert.FromBase64String("h9GeWnVeV2no4pkircgXAg==");
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

        public static byte[] GetStreamData(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    public static class ClientExtention
    {
        public static string ToTitleCaseWithSpace(this object data)
        {
            if (data != null)
                return Regex.Replace(data.ToString()!, @"(?<=[a-z])([A-Z])", @" $1");
            return string.Empty;
        }
    }

}
