using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using LibVLCSharp.Shared;

using Microsoft.EntityFrameworkCore;

namespace a.ViewModels;

public partial class CamViewModel : ObservableObject, IRecipient<NotiMessage>
{
    [ObservableProperty]
    public HttpCamClient cam;
    public MediaPlayer Player { get; private set; }
    private IMessenger Messenger { get; }

    //public Media media2;
    //public MediaPlayer Player2 { get; private set; }

    [ObservableProperty]
    public bool _isMaximized;

    [ObservableProperty]
    public string? _ipAddress;

    public CamViewModel(HttpCamClient camClient, IMessenger messenger)
{
    Messenger = messenger;
    Cam = camClient;
    IpAddress = camClient.Url;
    messenger.Register<NotiMessage>(this, (recipent, message) => Receive(message));

    // Add this CamViewModel to the camViewModels dictionary in TreeViewModel
    TreeViewModel.Instance.AddCamViewModel(IpAddress, this);
}
    public static async Task<CamViewModel> CreateAsync(HttpCamClient client, IMessenger messenger)
    {
        var camViewModel = new CamViewModel(client, messenger);
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
            var uri = new Uri(url ?? throw new ArgumentNullException(nameof(url)));
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
                Messenger.Send(new TempMessage(temp));
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
            Player = new MediaPlayer(media);
            Player.Play();

            //media2 = new Media(libVLC, vlRtspUrl, FromType.FromLocation);
            //media2.AddOption(":network-caching=100");
            //Player2 = new MediaPlayer(media2);
            //Player2.Play();
        });
    }

    public void Receive(NotiMessage message)
    {
        if (message.ip == IpAddress)
        {
            Messenger.Send(new MaxMessage(IsMaximized, this));
        }
        else
        {
        }
    }

    public void Maximize()
    {
        IsMaximized = true;
        Messenger.Send(new MaxMessage(IsMaximized, this));
    }
}

public record class MaxMessage(bool b, CamViewModel c);




