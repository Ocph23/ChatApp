using System.Text.Json;
using System.Text.RegularExpressions;

namespace Client
{
    public class Helper
    {


        public static string ServerURL => "https://localhost:7148";


        public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static string TitleCaseWithSpace(string p) => Regex.Replace(p, @"(?<=[a-z])([A-Z])", @" $1");

        public static IEnumerable<string> Roles = new List<string>() {
            "Admin",
            "Diakonia",
            "Accounting",
            "Rayon",
            "Jemaat",
        };


        public static IEnumerable<T> GetValues<T>(string text){

            return Enum.GetValues(typeof(T)).Cast<T>().Where(x=> x.ToString().ToLower().Contains(text.ToLower())).ToList();
        }


        public static string ConvertGolonganDarah(string darah)
        {
            if (darah.Contains("Plus"))
                return darah.Replace("Plus", "+");
            if (darah.Contains("Min"))
                return darah.Replace("Min", "-");
            return darah;
        }



    }

    public static class ClientExtention
    {
        public static string ToTitleCaseWithSpace(this object data)
        {

            
            if (data != null)
            {
                return Regex.Replace(data.ToString(), @"(?<=[a-z])([A-Z])", @" $1");
            }

            return string.Empty;
        }
    }

}
