using System.Windows.Input;

using a.ViewModels;

namespace a;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(HomeViewModel hvm, DataViewModel dvm, SetViewModel svm, EventViewModel evm)
    {
        InitializeComponent();
        HomeView.DataContext = hvm;
        DataView.DataContext = dvm;
        SetView.DataContext = svm;
        EventView.DataContext = evm;
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnClose));
    }

    private void OnClose(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
}
