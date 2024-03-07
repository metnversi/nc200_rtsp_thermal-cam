using System.Windows.Controls;

using a.ViewModels;

namespace a.Views;

/// <summary>
/// Interaction logic for SettingView.xaml
/// </summary>
public partial class SettingView : UserControl
{
    public SettingView()
    {
        InitializeComponent();
        DataContext = new SettingViewModel();
    }
}
