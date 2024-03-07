using System.Collections.ObjectModel;
using System.Windows;

using a.Models;
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

    [ObservableProperty]
    private string _username;
    [ObservableProperty]
    private string _password;

    //public ObservableCollection<Temp> source{ get; }

    public ConnectViewModel(HomeViewModel homeViewModel)
    {
        HomeViewModel = homeViewModel;
        Ip = "192.168.1.168";
        Username = "admin";
        Password = "admin123";
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
    private async Task Add()
    {
        var factory = new HttpCamClientFactory();
        var camClient = factory.Create(Ip, Username, Password);
        var camViewModel = await CamViewModel.CreateAsync(camClient);
        var camView = new CamView { DataContext = camViewModel };
        HomeViewModel.Panels.Add(camView);
    }

    public class HttpCamClientFactory
    {
        public HttpCamClient Create(string ip, string username, string password)
        {
            return new HttpCamClient(ip, username, password);
        }
    }
}

