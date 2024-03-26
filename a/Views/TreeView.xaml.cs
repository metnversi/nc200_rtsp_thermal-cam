using System.Windows;
using System.Windows.Controls;

using a.Models;

namespace a.Views;

/// <summary>
/// Interaction logic for TreeView.xaml
/// </summary>
public partial class TreeView : UserControl
{
    public TreeView()
    {
        InitializeComponent();
    }

    private async void TreeView_Loaded(object sender, RoutedEventArgs e)
    {
        await ((TreeViewModel)DataContext).LoadActiveCams();
    }
}
