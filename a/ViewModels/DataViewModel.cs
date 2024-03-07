using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace a.ViewModels;

public partial class DataViewModel : ObservableObject
{
    [ObservableProperty]
    private ChartViewModel _chartViewModel;
    public List<string> Labels { get; set; }
    public Func<double, string> YAxisFormatter { get; set; }
    public Func<double, string> YAxis2Formatter { get; set; }

    public ObservableCollection<string> Ips { get; set; }

    public CamViewModel CamViewModel { get; set; }

    public HttpCamClient client { get; set; }

    public ObservableCollection<Temp> Source { get; } = new ObservableCollection<Temp>();

    [ObservableProperty]
    public string? hello;

    public DataViewModel()
    {
        
        Hello = "vaifnsjfnbsffsdfbd";

        Source = new ObservableCollection<Temp>();
        Ips = new ObservableCollection<string> { "192.168.1.168", };
        ChartViewModel = new ChartViewModel(new ObservableCollection<Temp>(), Ips);
        Labels = new List<string>();
        YAxisFormatter = value => value.ToString("F1");
        YAxis2Formatter = value => value.ToString("F1");
        Source.CollectionChanged += DataCollection_CollectionChanged;
    }

    public void AddToSeries(Temp data)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var series = ChartViewModel.SeriesCollection.FirstOrDefault(s => s.Title == $"Temperature for ID {data.IpAddress}");

            if (series != null)
            {
                series.Values.Add(double.Parse(data.MaxTemp ?? "0"));
        }});
    }

    private void DataCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (Temp newItem in e.NewItems)
            {
                Labels.Add(DateTime.Now.ToString());
                AddToSeries(newItem);
            }
        }
    }

    public void AddTemp(Temp temp)
    {
        Source.Add(temp);
    }

    [RelayCommand]
    public void aaaaah()
    {
        AddTemp(new Temp { IpAddress = "192" });
    }
}
