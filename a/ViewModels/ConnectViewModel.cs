using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
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

    public ObservableCollection<Cam> Cams { get; set;} 

    public ConnectViewModel(HomeViewModel homeViewModel, CamDataContext context)
    {
        HomeViewModel = homeViewModel;
        Cams = new ObservableCollection<Cam>(context.Cams.ToList());
    }
    
    [RelayCommand]
    private void CloseWindow()
    {
        HomeViewModel.HideConnectView();
    }

    [RelayCommand]
    private async Task Add()
    {
        foreach(var cam in Cams)
        {
            if(cam.IsSelected == true && await IsHostAvailable(cam.IpAddress))
            {
                var factory = new HttpCamClientFactory();
                var camClient = factory.Create(cam.IpAddress, cam.Username, cam.Password);
                var camViewModel = await CamViewModel.CreateAsync(camClient);
                var camView = new CamView { DataContext = camViewModel };

                Application.Current.Dispatcher.Invoke(() =>
                {
                    HomeViewModel.Panels.Add(camView);
                });
            }
            else
            {
                MessageBox.Show($"Could not connect to IP {cam.IpAddress}");
            }
        }
    }

    private async Task<bool> IsHostAvailable(string host)
    {
        using (Ping ping = new Ping())
        {
            try
            {
                PingReply reply = await ping.SendPingAsync(host);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }

    
}

