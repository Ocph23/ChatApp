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

        public static string ServerURL => "https://chatapp.ocph23.tech/";
        //public static string ServerURL => "https://localhost:7148";
        public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static string TitleCaseWithSpace(string p) => Regex.Replace(p, @"(?<=[a-z])([A-Z])", @" $1");


       

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
