using System.Collections.ObjectModel;
using System.Windows;

using a.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace a.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public ConnectViewModel ConnectViewModel { get; }
    public ObservableCollection<CamView> Panels { get; }
    
    public HomeViewModel()
    {
        ConnectViewModel = new ConnectViewModel(this);
        Panels = new ObservableCollection<CamView>();
    }

    [RelayCommand]
    public void Show()
    {
        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            var con = new ConnectView(this);
            var window = new Window
            {
                Content = con,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }));
    }
    

}
