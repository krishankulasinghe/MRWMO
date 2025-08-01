using Microsoft.Extensions.Logging;
using MRWMO.Services;
using MRWMO.Shared.Services;

namespace MRWMO
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
               // .UsePancakeViewCompat()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("iskpota.ttf", "iskpota");
                    fonts.AddFont("iskpotab.ttf", "iskpotab");
                    fonts.AddFont("ARIALUNI.ttf", "ARIALUNI");
                   // fonts.AddFont("OpenSans.ttf", "OpenSansRegular");
                    fonts.AddFont("AbhayaLibre-Bold.ttf", "AbhayaLibreB");
                    fonts.AddFont("AbhayaLibre-Medium.ttf", "AbhayaLibreM");
                    fonts.AddFont("AbhayaLibre-Regular.ttf", "AbhayaLibreR");
                    fonts.AddFont("AbhayaLibre-SemiBold.ttf", "AbhayaLibreSB");
                });
            
            // Add device-specific services used by the MRWMO.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
