using System.Text.Json;
using System.Text.RegularExpressions;

namespace Shared
{
    public class Helper
    {

        //public static string ServerURL => "https://z016s16t-7148.asse.devtunnels.ms";
        public static string ServerURL => "https://localhost:7148";
        public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static string TitleCaseWithSpace(string p) => Regex.Replace(p, @"(?<=[a-z])([A-Z])", @" $1");

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
