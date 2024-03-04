using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using LibVLCSharp.Shared;

namespace a.ViewModels;

public partial class CamViewModel : ObservableObject
{
    [ObservableProperty]
    public HttpCamClient cam;
    
    public bool _isTemp;
    public bool IsTemp
    {
        get => _isTemp;
        set
        {
            _isTemp = value;
            OnPropertyChanged();
            SwitchMedia();
        }
    }

    public Media media;
    public Media media2;
    
    public MediaPlayer Player { get; private set; }

    private static HashSet<string>? usedIps;

    public CamViewModel(string camip, string Username, string Password)
    {
        if(camip is null)
        {
            throw new ArgumentNullException(nameof(camip));
        }
        Cam = new HttpCamClient(camip, $"{Username}", $"{Password}");
        usedIps ??= new HashSet<string>();
        if (usedIps.Contains(camip))
        {
            return;
            throw new ArgumentException("This IP address is already in use.");
        }
        usedIps.Add(camip);
    }

    public static async Task<CamViewModel> CreateAsync(string camip, string Username, string Password)
    {
        var camViewModel = new CamViewModel(camip, Username, Password);
        await camViewModel.Capture();
        return camViewModel;
    }

    public async Task Capture()
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            await Cam.A.Login();
            var (irRtspUrl, vlRtspUrl) = await Cam.F.OpenStream();
            var libVLC = new LibVLC();
            var media = new Media(libVLC, irRtspUrl, FromType.FromLocation);
            media.AddOption(":network-caching=100");  
            media2 = new Media(libVLC, vlRtspUrl, FromType.FromLocation);
            media2.AddOption(":network-caching=100");  
            Player = new MediaPlayer(media);

            Player.Play();
        });
    }
    private void SwitchMedia()
    {
        if (Player != null)
        {
            var oldMedia = Player.Media;
            Player.Stop();
            Player.Media = IsTemp ? media2 : media;
            Player.Play();
            oldMedia.Dispose();  // Dispose of the old media
        }
    }
    
}