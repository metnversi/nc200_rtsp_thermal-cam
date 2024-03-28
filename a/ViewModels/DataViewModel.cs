using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using LiveCharts;
using LiveCharts.Wpf;

namespace a.ViewModels;

public partial class DataViewModel : ObservableObject, IRecipient<TempMessage>
{
    public ObservableCollection<Temp> Source { get; set; }
    private CamDataContext context { get; set; }

    [ObservableProperty]
    private ChartViewModel _chartViewModel;

    public List<string> Labels { get; set; }

    public Func<double, string> YAxisFormatter { get; set; }

    public ObservableCollection<string> Ips { get; set; }
    private CancellationTokenSource? _refreshCancellation;


    public DataViewModel(IMessenger messenger)
    {
        context = new CamDataContext();
        messenger.Register<TempMessage>(this, (recipent, message) => AddTemp(message.Temp).GetAwaiter());
        Source = new ObservableCollection<Temp>(context.Temps.OrderByDescending(t => t.Time).Take(50).ToList());
        Ips = new ObservableCollection<string>(Source.Select(t => t.IpAddress ?? throw new ArgumentNullException(nameof(IPAddress))).Distinct());
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

    public async Task AddTemp(Temp temp)
    {
        await context.Temps.AddAsync(temp);
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
    [RelayCommand]
    private async Task OnRefresh()
    {
        CancellationTokenSource cts = new();
        Interlocked.Exchange(ref _refreshCancellation, cts)?.Cancel();
        try
        {
            Source.Clear();
            await Task.Delay(10, cts.Token);
        }
        catch (OperationCanceledException)
        { }
    }

    public void Receive(TempMessage message)
    {
        AddTemp(message.Temp).GetAwaiter();
    }
}
