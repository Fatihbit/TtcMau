using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using TtcMau.ViewModels;

namespace TtcMau
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var cookieContainer = new System.Net.CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
                UseProxy = false
            };

            builder.Services.AddSingleton(new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7143")
            });

            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<LadingViewModel>();
            builder.Services.AddSingleton<StatusLadingViewModel>();
            builder.Services.AddSingleton<ShipStatusViewModel>(); // Add ShipStatusViewModel
            builder.Services.AddSingleton<VeiligheidsChecklistViewModel>();
            builder.Services.AddSingleton<ShipRegistrationViewModel>();
            builder.Services.AddSingleton<TerminalRegistrationViewModel>();
            builder.Services.AddSingleton<OverviewViewModel>();
            builder.Services.AddSingleton<ShipSearchViewModel>();



            builder.Services.AddLogging();


            
            return builder.Build();
        }
    }
}
