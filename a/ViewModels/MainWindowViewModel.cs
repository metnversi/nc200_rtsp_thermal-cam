using a.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

namespace a;

public partial class MainWindowViewModel : ObservableObject
{
    public HomeViewModel HomeViewModel { get; }
    public SettingViewModel SettingViewModel { get; }
    
    public MainWindowViewModel()
    {
        HomeViewModel = new HomeViewModel();
        SettingViewModel = new SettingViewModel();
    }

}
