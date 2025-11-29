using Microsoft.Extensions.Logging;
using TrainingDiary.Services;
using TrainingDiary.Views;
using TrainingDiary.ViewModels;

namespace TrainingDiary
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<TrainingDatabase>();

            builder.Services.AddSingleton<HomePageViewModel>();
            builder.Services.AddSingleton<HomePage>();

            builder.Services.AddTransient<EditTrainingPageViewModel>();
            builder.Services.AddTransient<EditTrainingPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
