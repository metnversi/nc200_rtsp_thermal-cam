using System.Collections.ObjectModel;
using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace a.ViewModels;

public partial class DataViewModel : ObservableObject
{
    
    public CamViewModel CamViewModel { get; set; }

    public ObservableCollection<Temp> Source { get; } = new ObservableCollection<Temp>();
    
    public DataViewModel()
    {
        Messenger.Instance.TempAdded += AddTemp;
    }

    public void AddTemp(Temp temp)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Source.Add(temp);
        });
    }

    //[RelayCommand]
    //private async Task OnRefresh()
    //{
    //    CancellationTokenSource cts = new();
    //    Interlocked.Exchange(ref _refreshCancellation, cts)?.Cancel();
    //    try
    //    {
    //        Rentals.Clear();
    //        await Task.Delay(0, cts.Token);
    //        // await foreach (var rental in Context.Rentals.Where(x => x.ReturnedOn == null).AsAsyncEnumerable().WithCancellation(cts.Token))
    //        // {
    //        //     Rentals.Add(RentalViewModelFactory(rental));
    //        // }
    //    }
    //    catch(OperationCanceledException)
    //    { }
    //}

}
