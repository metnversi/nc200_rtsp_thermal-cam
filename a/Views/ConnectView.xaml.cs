using System.Windows.Controls;

using a.ViewModels;

namespace a.Views;

/// <summary>
/// Interaction logic for ConnectView.xaml
/// </summary>
public partial class ConnectView 
{
    public ConnectView(HomeViewModel homeViewModel)
    {
        InitializeComponent();
        DataContext = new ConnectViewModel(homeViewModel);
    }
}
