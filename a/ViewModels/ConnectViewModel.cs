using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace a.ViewModels;

public partial class ConnectViewModel : ObservableObject
{
    [ObservableProperty]
    private HomeViewModel _homeViewModel;
    private IMessenger Messenger { get; }

    public ObservableCollection<Cam> Cams { get; set;} 
    
    public ConnectViewModel(HomeViewModel homeViewModel, CamDataContext context, IMessenger messenger)
    {
        HomeViewModel = homeViewModel;
        Cams = new ObservableCollection<Cam>(context.Cams.ToList());
        Messenger = messenger;
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
                var camViewModel = await CamViewModel.CreateAsync(camClient, Messenger);

                using(var context = new CamDataContext())
                {
                    var active = new Active { 
                        AreaId=cam.AreaId, 
                        IpAddress = cam.IpAddress, 
                        IsActive = true };
                    context.Actives.Add(active);
                    await context.SaveChangesAsync();

                    Messenger.Send(new ActiveUpdates(active));
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    HomeViewModel.Panels.Add(camViewModel);
                });
            }
            else
            {
                MessageBox.Show($"Could not connect to IP {cam.IpAddress}");
            }
        }
    }


    [RelayCommand]
    public async Task AddOne()
    {
        using (var context = new CamDataContext())
        {

        }
    }

    private async Task<bool> IsHostAvailable(string host)
    {
        using (Ping ping = new Ping())
        {
            try
            {
                PingReply reply = await ping.SendPingAsync(host);
                //IsActive = true;
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
            
        }
    }


}

