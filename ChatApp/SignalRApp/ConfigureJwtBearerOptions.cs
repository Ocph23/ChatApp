using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace ChatApp.SignalRApp
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            if (options.Events != null)
            {


                var originalOnMessageReceived = options.Events.OnMessageReceived;
                options.Events.OnMessageReceived = async context =>
                {
                    await originalOnMessageReceived(context);

                    if (string.IsNullOrEmpty(context.Token))
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/chathub"))
                        {
                            context.Token = accessToken;
                        }
                    }
                    else
                    {

                    }
                };
            }
        }



    }
}
