using System.Collections.ObjectModel;
using System.Windows;

using a.Models;
using a.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace a.ViewModels;

public partial class HomeViewModel : ObservableObject, IRecipient<MaxMessage>
{
    public TreeViewModel treeViewModel { get; set; }
    public ConnectView ConnectView { get; private set; }
    public ConnectViewModel ConnectViewModel { get; }
    public ObservableCollection<CamViewModel> Panels { get; }

    [ObservableProperty]
    private bool _isConnectViewOpen;
    
    
    public HomeViewModel(CamDataContext context, IMessenger messenger)
    {
        ConnectViewModel = new ConnectViewModel(this, context, messenger);
        treeViewModel = new TreeViewModel(messenger);
        ConnectView = new ConnectView { DataContext = ConnectViewModel };
        Panels = new ObservableCollection<CamViewModel>();
        messenger.Register<MaxMessage>(this, (recipient, message) => Receive(message));
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
    public void Receive(MaxMessage msg)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (var panel in Panels)
            {
                panel.IsMaximized = false;
            }

            var senderPanel = Panels.FirstOrDefault(p => p == msg.c);
            if (senderPanel != null)
            {
                senderPanel.IsMaximized = true;
            }
        });
    }
}
