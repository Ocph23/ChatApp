
using System.Security.Claims;
using System.Text;

namespace MarampaApp.Client
{
    public static class MarampaClientExtention
    {

        public static bool IsInRole(this IEnumerable<ClaimsIdentity> identities, string role)
        {
            for (int i = 0; i < identities.Count(); i++)
            {
                if (identities.ToArray()[i] != null)
                {
                    if (identities.ToArray()[i].HasClaim(identities.ToArray()[i].RoleClaimType, role))
                    {
                        return true;
                    }
                }
            }

            return false;
        }




    }



}
