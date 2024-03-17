using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using LibVLCSharp.Shared;

namespace a.ViewModels;

public partial class CamViewModel : ObservableObject
{
    [ObservableProperty]
    public HttpCamClient cam;
    public Media media;
    public Media media2;
    public MediaPlayer Player { get; private set; }

    [ObservableProperty]
    public string? ipAddress;

    public CamViewModel(HttpCamClient camClient)
    {
        Cam = camClient;
        IpAddress = camClient.Url; 
    }

    public static async Task<CamViewModel> CreateAsync(HttpCamClient client)
    {
        var camViewModel = new CamViewModel(client);
        await camViewModel.Capture();
        return camViewModel;
    }

    public async Task Capture()
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            await Cam.A.Login();
            var (irRtspUrl, vlRtspUrl) = await Cam.F.OpenStream();
            var (url, min, max) = await Cam.F.GetRealTimeTemp();
            var temp = new Temp 
            { 
                IpAddress = url, 
                MinTemp = min, 
                MaxTemp = max, 
                TimeReading = DateTime.Now 
            };
            Messenger.Instance.OnTempAdded(temp);

            var libVLC = new LibVLC();
            var media = new Media(libVLC, irRtspUrl, FromType.FromLocation);
            media.AddOption(":network-caching=100");
            media2 = new Media(libVLC, vlRtspUrl, FromType.FromLocation);
            media2.AddOption(":network-caching=100");
            Player = new MediaPlayer(media);
            Player.Play();
        });
    }
}

