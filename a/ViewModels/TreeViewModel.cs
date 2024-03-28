using System.Collections.ObjectModel;
using System.Windows.Data;

using a.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace a.Models;

public partial class TreeViewModel : ObservableObject, IRecipient<ActiveUpdates>
{
    private CancellationTokenSource? _refreshCancellation;
    public ObservableCollection<IGrouping<int?, Active>> ActiveCams { get; } = new ObservableCollection<IGrouping<int?, Active>>();

    public IMessenger Messenger { get; }
    public static TreeViewModel Instance { get; private set; }

    public TreeViewModel(IMessenger messenger) 
    {
        Instance = this;
        Messenger = messenger;
        DeleteRecord().GetAwaiter();
        messenger.Register<ActiveUpdates>(this, (recipent, message) => LoadActiveCams().GetAwaiter());
        BindingOperations.EnableCollectionSynchronization(ActiveCams, new());
    }

    public async Task LoadActiveCams()
    {
        await OnRefresh();
        using (var context = new CamDataContext())
        {
            var actives = context.Actives.Where(a => a.IsActive).ToList();
            var groupedActives = actives.GroupBy(a => a.AreaId).ToList();
            foreach (var group in groupedActives)
            {
                ActiveCams.Add(group);
            }
        }
    }

    public async Task DeleteRecord()
    {
        using (var context = new CamDataContext())
        {
            context.Actives.RemoveRange(context.Actives);
            await context.SaveChangesAsync();
        }
    }
    [RelayCommand]
    private async Task OnRefresh()
    {
        CancellationTokenSource cts = new();
        Interlocked.Exchange(ref _refreshCancellation, cts)?.Cancel();
        try
        {
            ActiveCams.Clear();
            await Task.Delay(10, cts.Token);
        }
        catch (OperationCanceledException)
        { }
    }

    public async void Receive(ActiveUpdates message)
    {
        await RefreshCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    public async Task Maxim(string ipAddress)
    {
        await RefreshCommand.ExecuteAsync(null);
        ipAddress = "http://" + ipAddress +  "/getmsginfo";
        //SendMaximizeMessage(ipAddress);
        Messenger.Send(new NotiMessage(ipAddress));
    }

    private Dictionary<string, CamViewModel> camViewModels = new Dictionary<string, CamViewModel>();

    public void AddCamViewModel(string ipAddress, CamViewModel viewModel)
    {
        camViewModels[ipAddress] = viewModel;
    }

    public void SendMaximizeMessage(string ipAddress)
    {
        if (camViewModels.TryGetValue(ipAddress, out var viewModel))
        {
            viewModel.Maximize();
        }
    }
}

public record class NotiMessage(string ip);