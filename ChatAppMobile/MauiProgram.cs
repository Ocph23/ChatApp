using ChatAppMobile.Mocks;
using ChatAppMobile.Models;
using ChatAppMobile.Services;
using Client;
using CommunityToolkit.Maui;
using MarampaApp.Client.OcphAuthClient;
using Microsoft.Extensions.Logging;
using OcphApiAuth.Client;
using Shared;
using Shared.Contracts;

namespace ChatAppMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FASolid");
                });


            builder.Services.AddOcphAuthClient(Helper.ServerURL);
            builder.Services.AddClientService();



#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }

        public static IServiceCollection AddClientService(this IServiceCollection services)
        {
            services.AddScoped<ChatClient>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IFileServices, FileService>();

            return services;
        }
    }


}
