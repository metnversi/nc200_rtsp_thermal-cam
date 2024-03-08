using System.Collections.ObjectModel;
using System.Windows;

using a.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace a.ViewModels;

public partial class DataViewModel : ObservableObject
{
    
    public CamViewModel CamViewModel { get; set; }

    public ObservableCollection<Temp> Source { get; } = new ObservableCollection<Temp>();
    
    public DataViewModel()
    {
        Messenger.Instance.TempAdded += AddTemp;
    }

    public void AddTemp(Temp temp)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Source.Add(temp);
        });
    }

}
