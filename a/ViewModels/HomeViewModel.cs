using System.Collections.ObjectModel;
using System.Windows;

using a.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using OpenCvSharp;

namespace a.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public ConnectViewModel ConnectViewModel { get; }
    public ObservableCollection<VideoCapture> Feeds { get; } 

    public HomeViewModel()
    {
        ConnectViewModel = new ConnectViewModel();
        Feeds = new ObservableCollection<VideoCapture>();
    }

    [RelayCommand]
    public void AddVideoCapture()
    {
        var capture = new VideoCapture(0);
        Feeds.Add(capture);
    }

    [RelayCommand]
    public void Show()
    {
        var con = new ConnectView();
        var window = new System.Windows.Window
        {
            Content = con,
            SizeToContent = SizeToContent.WidthAndHeight, //this line is important
            WindowStyle = WindowStyle.None, // This will remove the title bar and system buttons
            ResizeMode = ResizeMode.NoResize,
            WindowStartupLocation = WindowStartupLocation.CenterOwner // This will center the window
        };
        window.Owner = Application.Current.MainWindow; // Set the owner to the current window
        window.ShowDialog();
    }

}
