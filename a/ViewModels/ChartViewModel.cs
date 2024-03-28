using System.Collections.ObjectModel;
using a.Models;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;

using LiveCharts;
using LiveCharts.Wpf;
using System.Diagnostics;
using System.Windows;
using Timer = System.Timers.Timer;

namespace a.ViewModels;

public partial class ChartViewModel : ObservableObject
{
    public Timer timer;
    [ObservableProperty]
    private SeriesCollection _seriesCollection;

    [ObservableProperty]
    private SeriesCollection _memoryChartSeries;

    public ChartViewModel(ObservableCollection<Temp> data, ObservableCollection<string> ips)
    {
        SeriesCollection = new SeriesCollection();

        foreach (var ip in ips)
        {
            var dataForId = data.Where(d => d.IpAddress == ip).ToList();

            LineSeries series = new LineSeries
            {
                Title = $"Temperature for IP {ip}",
                Values = new ChartValues<double>(dataForId.Where(d => !string.IsNullOrEmpty(d.MaxTemp)).Select(d => double.Parse(d.MaxTemp ?? "0"))),
                PointGeometry = DefaultGeometries.Circle
            };

            SeriesCollection.Add(series);
        }

        MemoryChartSeries = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Memory",
                Values = new ChartValues<double> { },
                StrokeThickness = 2,
                PointGeometry = null
            }
        };

        timer = new Timer(1000);
        timer.Elapsed += UpdateMemoryChart;
        timer.Start();
    }

    private void UpdateMemoryChart(object? sender, ElapsedEventArgs e)
    {
        //try
        //{
        //    var memoryUsage = Process.GetCurrentProcess().PrivateMemorySize64 / (1024.0 * 1024.0);
        //    if (Application.Current != null)
        //    {
        //        Application.Current.Dispatcher.Invoke(() =>
        //        {
        //            if (MemoryChartSeries != null && MemoryChartSeries.Count > 0 && MemoryChartSeries[0].Values != null)
        //            {
        //                MemoryChartSeries[0].Values.Add(memoryUsage);
        //                if (MemoryChartSeries[0].Values.Count > 60)
        //                {
        //                    MemoryChartSeries[0].Values.RemoveAt(0);
        //                }
        //            }
        //        });
        //    }
        //}
        //catch (ArgumentOutOfRangeException)
        //{
        //}
    }
}