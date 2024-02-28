using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;
using OpenCvSharp.WpfExtensions;

using OpenCvSharp;
using System.Windows;

namespace a.ViewModels;

public partial class CamViewModel : ObservableObject
{
    public string Ip { get; }
    public Mat Frame { get; private set; }
    public BitmapSource DisplayFrame { get; private set; }
    public VideoCapture capture;
    private static HashSet<string> usedIps = new HashSet<string>();
    public CamViewModel(string ip)
    {
        if (usedIps.Contains(ip))
        {
            throw new ArgumentException("This IP address is already in use.");
        }
        Ip = ip;
        capture = new VideoCapture(ip);
        Frame = new Mat();
        Task.Run(() => Capture());
    }

    public void Capture()
    {
        while(true)
        {
            capture.Read(Frame);
            if ( Frame.Rows > 0 && Frame.Cols > 0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DisplayFrame = Frame.ToBitmapSource();
                    OnPropertyChanged(nameof(DisplayFrame)); // Notify the UI that DisplayFrame has changed
                });
            }
        }
    }
}
