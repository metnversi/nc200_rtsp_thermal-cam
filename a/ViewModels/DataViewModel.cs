using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using LiveCharts;
using LiveCharts.Wpf;

namespace a.ViewModels;

public partial class DataViewModel : ObservableObject
{
    public ObservableCollection<Temp> Source { get; set; }
    private CamDataContext context { get; set; }

    [ObservableProperty]
    private ChartViewModel _chartViewModel;

    public List<string> Labels { get; set; }

    public Func<double, string> YAxisFormatter { get; set; }

    public ObservableCollection<string> Ips { get; set; }


    public DataViewModel()
    {
        context = new CamDataContext();
        Messenger.Instance.TempAdded += AddTemp;
        Source = new ObservableCollection<Temp>(context.Temps.OrderByDescending(t => t.Time).Take(50).ToList());

        Ips = new ObservableCollection<string>(Source.Select(t => t.IpAddress).Distinct());

        ChartViewModel = new ChartViewModel(Source, Ips);

        Labels = new List<string>();
        YAxisFormatter = value => value.ToString("F1");

        Source.CollectionChanged += (s, e) => 
    {
        if (e.NewItems != null)
        {
            foreach(Temp newItem in e.NewItems)
            {
                Labels.Add(DateTime.Now.ToString());
                AddToSeries(newItem);
            }
        }

        if (e.OldItems != null)
        {
            foreach(Temp oldItem in e.OldItems)
            {
                RemoveFromSeries(oldItem);
            }
        }
    };
    }

    public void AddTemp(Temp temp)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Source.Add(temp);
        });
    }

    public void AddToSeries(Temp data)
{
    var series = ChartViewModel.SeriesCollection.FirstOrDefault(s => s.Title == $"Temperature for IP {data.IpAddress}");

    if (series != null)
    {
        series.Values.Add(double.Parse(data.MaxTemp ?? "0"));

        // Only keep the last 50 values
        while (series.Values.Count > 50)
        {
            series.Values.RemoveAt(0);
        }
    }
    else
    {
        // If the series does not exist, create a new one
        LineSeries newSeries = new LineSeries
        {
            Title = $"Temperature for IP {data.IpAddress}",
            Values = new ChartValues<double> { double.Parse(data.MaxTemp ?? "0") },
            PointGeometry = DefaultGeometries.Circle
        };

        ChartViewModel.SeriesCollection.Add(newSeries);
    }
}

public void RemoveFromSeries(Temp data)
{
    var series = ChartViewModel.SeriesCollection.FirstOrDefault(s => s.Title == $"Temperature for IP {data.IpAddress}");

    if (series != null)
    {
        series.Values.RemoveAt(0);

        // If the series is empty, remove it
        if (series.Values.Count == 0)
        {
            ChartViewModel.SeriesCollection.Remove(series);
        }
    }
}

    private void DataCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (Temp newItem in e.NewItems)
            {
                AddToSeries(newItem);
            }
        }
    }
}
