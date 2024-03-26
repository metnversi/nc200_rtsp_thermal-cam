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
    public string? _testString;

    

    [ObservableProperty]
    private bool _isConnectViewOpen;
    
    
    public HomeViewModel(CamDataContext context, IMessenger messenger)
    {
        ConnectViewModel = new ConnectViewModel(this, context, messenger);
        treeViewModel = new TreeViewModel(messenger);
        ConnectView = new ConnectView { DataContext = ConnectViewModel };
        Panels = new ObservableCollection<CamViewModel>();
        messenger.Register<MaximizeMessage>(this, (recipient, message) => ExpandCam(message.ip));
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

    // private void ExpandCam(string ipAddress)
    // {
    //     var camViewModel = Panels.FirstOrDefault(c => c.IpAddress == ipAddress);
    //     if (camViewModel != null)
    //     {
            
    //     }
    // }

    private void ExpandCam(string ipAddress)
    {
        foreach (var panel in Panels)
        {
            panel.IsExpanded = panel.IpAddress == ipAddress;
        }
    }

    public void Receive(MaximizeMessage message)
    {
        
    }
}
