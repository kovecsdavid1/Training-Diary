using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace TrainingDiary.Models;

public partial class Training : ObservableObject
{
    [ObservableProperty]
    [property: PrimaryKey]
    [property: AutoIncrement]
    public int id;

    [ObservableProperty]
    public string name;

    [ObservableProperty]
    public string trainedRegion;

    [ObservableProperty]
    public string type;

    [ObservableProperty]
    public string intensivity;

    [ObservableProperty]
    public DateTime startTime;

    [ObservableProperty]
    public DateTime endTime;


    public Training GetCopy()
    {
        return (Training)this.MemberwiseClone();
    }
}