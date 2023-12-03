using ChatApp.Models;
using Shared;
using System.Security.Claims;

namespace ChatApp
{
    public static class ChatAppExtention
    {

        public static IEnumerable<TemanDTO> ToAnggotaDTO(this IEnumerable<AnggotaGroup> list)
        {
            return list.Select(x => new TemanDTO() { Email=x.Anggota.Email, Keanggotaan=x.Keanggotaan, Nama=x.Anggota.Name, TemanId=x.Anggota.Id });
        }

        public static string? GetUserId (this ClaimsPrincipal claim)
        {
            var user = claim.Claims.FirstOrDefault(x => x.Type == "id");
            if(user == null)
                return string.Empty;
            return user.Value;
        }


    }
}
