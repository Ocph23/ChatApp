using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OcphApiAuth.Client;
public class OcphAuthStateProvider
{

    public async Task<ClaimsPrincipal> GetAuthenticationStateAsync()
    {
        string? token = Preferences.Get("token", string.Empty);
        return await GetAuthenticationStateAsync(token);
    }


    public async Task<ClaimsPrincipal> GetAuthenticationStateAsync(string? token)
    {
        IdentityModelEventSource.ShowPII = true;
        ClaimsIdentity identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        if (string.IsNullOrEmpty(token))
            return user;
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(token));
            string secret = "O0ywQA1xv4h1EXL4cnsQ4ReHEoqwMuejtXB4KxOeTCB4nOpV4yVegegbEgtzsZWfl5wlBWBY5kpUPVsEmkVr3V9sxvOPmT4YR3PvUCiw7s1xMrSoCMVqnG7VlSeO469Z";
            string issuer = "https://chatapp.apspapua.com/";
            await Task.Delay(100);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var key = Encoding.ASCII.GetBytes(secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

            }, out validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            ArgumentNullException.ThrowIfNull(jwtToken);
            string id = jwtToken.Claims.FirstOrDefault(x => x.Type == "id")!.Value;
            var name = jwtToken.Claims.FirstOrDefault(x => x.Type == "name")!.Value;
            var sub = jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")!.Value;
            var email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")!.Value;
            var roles = jwtToken.Claims.Where(x => x.Type == "role").ToList();
            var claims = new List<Claim>
                {
                               new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                               new Claim(ClaimTypes.Name, name),
                               new Claim(ClaimTypes.Email, email),
                };

            foreach (var claim in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, claim.Value));
            }
            identity = new ClaimsIdentity(claims, "jwt");
            user = new ClaimsPrincipal(identity);
            return user;
        }
        catch (Exception)
        {
            return user;
        }
    }

    public async Task<string> GetUserId()
    {
        var result = await this.GetAuthenticationStateAsync();
        if (result.Identity.IsAuthenticated)
        {
            var claim= result.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier"));
            if(claim != null)
            {
                return claim.Value;
            }
        }
        return string.Empty;
    }
}
