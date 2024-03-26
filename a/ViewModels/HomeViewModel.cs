using System.Collections.ObjectModel;

using a.Models;
using a.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace a.ViewModels;

public partial class HomeViewModel : ObservableObject, IRecipient<MaximizeMessage>
{
    public TreeViewModel treeViewModel { get; set; }
    public ConnectView ConnectView { get; private set; }
    public ConnectViewModel ConnectViewModel { get; }
    public ObservableCollection<CamViewModel> Panels { get; }

    [ObservableProperty]
    public CamViewModel? _selectedCam;

    [ObservableProperty]
    private bool _isConnectViewOpen;
    
    
    public HomeViewModel(CamDataContext context, IMessenger messenger)
    {
        ConnectViewModel = new ConnectViewModel(this, context, messenger);
        treeViewModel = new TreeViewModel(messenger);
        ConnectView = new ConnectView { DataContext = ConnectViewModel };
        Panels = new ObservableCollection<CamViewModel>();
        messenger.Register<MaximizeMessage>(this, (recipent, message) => Receive(message));
    }
    [RelayCommand]
    public void Show()
    {
        IsConnectViewOpen = true;
    }
    public void HideConnectView()
    {
        IsConnectViewOpen = false;
    }


    public void Receive(MaximizeMessage message)
    {
        SelectedCam = message.Content;
    }
}
