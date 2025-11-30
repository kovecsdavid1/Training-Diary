using TrainingDiary2.Views;

namespace TrainingDiary2;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("trainingdetails", typeof(TrainingDetailsPage));
        Routing.RegisterRoute("edittraining", typeof(EditTrainingPage));
        Routing.RegisterRoute("statistics", typeof(StatisticsPage));
    }
}