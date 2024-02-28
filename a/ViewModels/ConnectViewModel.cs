using System.Windows;

using a.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace a.ViewModels;

public partial class ConnectViewModel : ObservableObject
{
    [ObservableProperty]
    private HomeViewModel _homeViewModel;
    [ObservableProperty]
    private string _ip;
    public ConnectViewModel(HomeViewModel homeViewModel)
    {
        HomeViewModel = homeViewModel;
        Ip = "http://192.168.1.47:4747/video";
    }
    [RelayCommand]
    private void CloseWindow(object parameter)
    {
        if (parameter is Window window)
        {
            window.Close();
        }
    }

    [RelayCommand]
    private void Add()
    {
        var camViewModel = new CamViewModel(Ip);
        var camView = new CamView { DataContext = camViewModel };
        HomeViewModel.Panels.Add(camView);
    }
}
