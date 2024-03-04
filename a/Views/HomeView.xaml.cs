﻿using System.Windows.Controls;
using a.ViewModels;

namespace a.Views;


/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        DataContext = new HomeViewModel();
    }
}
