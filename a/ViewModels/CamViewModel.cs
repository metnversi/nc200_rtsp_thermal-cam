using System.Collections.ObjectModel;
using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using LibVLCSharp.Shared;

namespace a.ViewModels;

public partial class CamViewModel : ObservableObject
{
    [ObservableProperty]
    public HttpCamClient cam;
    private System.Timers.Timer timer;
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
    public DataViewModel DataViewModel { get; set;}
    public string? ipAddress;

    public CamViewModel(HttpCamClient camClient)
    {
        DataViewModel = new DataViewModel();
        if(camClient is null)
        {
            throw new ArgumentNullException(nameof(camClient));
        }
        Cam = camClient;
        ipAddress = Cam.Url; 
        
        usedIps ??= new HashSet<string>();
        if (usedIps.Contains(ipAddress))
        {
            throw new ArgumentException("This IP address is already in use.");
        }
        usedIps.Add(ipAddress);
    }

    public static async Task<CamViewModel> CreateAsync(HttpCamClient camClient)
    {
        var camViewModel = new CamViewModel(camClient);
        await camViewModel.Capture();
        return camViewModel;
    }

    public async Task Capture()
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            await Cam.A.Login();
            var (irRtspUrl, vlRtspUrl) = await Cam.F.OpenStream();
            if (timer == null)
            {
                timer = new System.Timers.Timer(1000); 
                timer.Elapsed += async (sender, e) => await GetRealTimeTemperatureAsync();
                timer.Start();
            }
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
            oldMedia.Dispose(); 
        }
    }
    private async Task<Temp> GetRealTimeTemperatureAsync()
    {
        var (min, max) = await Cam.F.GetRealTimeTemp();
        var temp = new Temp
        {
            IpAddress = ipAddress,
            TimeReading = DateTime.Now,
            MinTemp = min,
            MaxTemp = max,
        };
        temp.PropertyChanged += (sender, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"Property {e.PropertyName} changed");
        };
        Application.Current.Dispatcher.Invoke(() =>
        {
            DataViewModel.AddTemp(temp);
        });
        return temp;
    }
    
}