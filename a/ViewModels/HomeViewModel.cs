using System.Collections.ObjectModel;
using System.Windows;

using a.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace a.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public ConnectView ConnectView { get; private set; }
    public ConnectViewModel ConnectViewModel { get; }
    public ObservableCollection<CamView> Panels { get; }

    private bool _isConnectViewOpen;
    public bool IsConnectViewOpen
    {
        get => _isConnectViewOpen;
        set => SetProperty(ref _isConnectViewOpen, value);
    }
    
    public HomeViewModel(CamDataContext context)
    {
        ConnectViewModel = new ConnectViewModel(this, context);
        ConnectView = new ConnectView { DataContext = ConnectViewModel };
        Panels = new ObservableCollection<CamView>();
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

}
