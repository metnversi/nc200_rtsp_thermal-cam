using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using LibVLCSharp.Shared;

using Microsoft.EntityFrameworkCore;

namespace a.ViewModels;

public partial class CamViewModel : ObservableObject
{
    [ObservableProperty]
    public HttpCamClient cam;
    public Media media;
    public Media media2;
    public MediaPlayer Player { get; private set; }
    public MediaPlayer Player2 { get; private set; }

    [ObservableProperty]
    public string? ipAddress;

    public CamViewModel(HttpCamClient camClient) // dont init the context for constructor, aaaah
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
            var uri = new Uri(url);
            var ipAddress = uri.Host;
            var timer = new System.Timers.Timer(3000);
            timer.Elapsed += async (sender, e) => 
            {
                var temp = new Temp
                {
                    IpAddress = ipAddress,
                    MinTemp = min,
                    MaxTemp = max,
                    Time = DateTime.Now
                };
                Messenger.Instance.OnTempAdded(temp);
                using (var context = new CamDataContext())
                {
                    var camera = await context.Cams.FirstOrDefaultAsync(c => c.IpAddress == ipAddress);
                    if (camera == null)
                    {
                        camera = new Cam { IpAddress = ipAddress };
                        context.Cams.Add(camera);
                        await context.SaveChangesAsync();
                    }
                    context.Temps.Add(temp);
                    await context.SaveChangesAsync();
                }
            };
            timer.AutoReset = true;
            timer.Enabled = true;

            var libVLC = new LibVLC();
            var media = new Media(libVLC, irRtspUrl, FromType.FromLocation);
            media.AddOption(":network-caching=100");
            //media2 = new Media(libVLC, vlRtspUrl, FromType.FromLocation);
            //media2.AddOption(":network-caching=100");
            Player = new MediaPlayer(media);
            Player.Play();
            //Player2 = new MediaPlayer(media2);
            //Player2.Play();
        });
    }
}

