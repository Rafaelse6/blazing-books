using BlazingBooks.Mobile.Services;
using BlazingBooks.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Refit;


namespace BlazingBooks.Mobile
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

            builder.Services.AddTransient<IBookService, ApiBookFetcher>();

            ConfigureRefit(builder.Services);

            return builder.Build();
        }

        private static void ConfigureRefit(IServiceCollection services)
        {
            var refitSettings = new RefitSettings
            {
                HttpMessageHandlerFactory = () =>
                {
#if ANDROID
                    return new Xamarin.Android.Net.AndroidMessageHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, certificate, chain, sslPolicyErrors) =>
                            certificate?.Issuer == "CN=localhost"
                            || sslPolicyErrors == System.Net.Security.SslPolicyErrors.None

                    };
#elif IOS
                    return new NSUrlSessionHandler
                    {
                        TrustOverrideForUrl = (nSUrlSessionHandler, url, sectTrust) => url.StartsWith("https://localhost")
                    };
#endif
                    return null;
                }
            };

            services.AddRefitClient<IBookApi>(refitSettings)
                    .ConfigureHttpClient(httpClient =>
                    {
                        var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                                    ? "https://10.0.2.2:7115"
                                    : "http://locahost:7115";

                        httpClient.BaseAddress = new Uri(baseUrl);
                    });
        }
    }
}
