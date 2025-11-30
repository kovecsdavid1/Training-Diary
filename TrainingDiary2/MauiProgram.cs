using Microsoft.Extensions.Logging;
using TrainingDiary2.Converters;
using TrainingDiary2.Services;
using TrainingDiary2.ViewModels;
using TrainingDiary2.Views;

namespace TrainingDiary2;

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
        builder.Services.AddSingleton<HomePageViewModel>();
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddTransient<EditTrainingPageViewModel>();
        builder.Services.AddTransient<EditTrainingPage>();
        builder.Services.AddTransient<StatisticsPageViewModel>();
        builder.Services.AddTransient<StatisticsPage>();
        builder.Services.AddSingleton<ITrainingDatabase, SQLiteTrainingDatabase>();
        builder.Services.AddTransient<DurationCalculator>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}