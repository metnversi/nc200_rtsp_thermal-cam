using System.Windows.Controls;

using a.ViewModels;

namespace a.Views;

/// <summary>
/// Interaction logic for ConnectView.xaml
/// </summary>
public partial class ConnectView : UserControl
{
    public ConnectView()
    {
        InitializeComponent();
        DataContext = new ConnectViewModel();
    }
    
}
