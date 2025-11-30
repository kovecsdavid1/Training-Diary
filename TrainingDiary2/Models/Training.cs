using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.ComponentModel.DataAnnotations;

namespace TrainingDiary2.Models;

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
    public DateTime date;

    [ObservableProperty]
    public TimeSpan startTime;

    [ObservableProperty]
    public TimeSpan endTime;

    [ObservableProperty]
    public string imagePath;

    [Ignore]
    public TimeSpan Duration => EndTime - StartTime;

    public Training GetCopy()
    {
        return (Training)this.MemberwiseClone();
    }

    public override string ToString()
    {
        return $"Check out my training: {Name} ({Type}) on {Date:yyyy.MM.dd}. Trained {TrainedRegion} for {Duration:hh\\:mm}. It was {Intensivity}!";
    }
}