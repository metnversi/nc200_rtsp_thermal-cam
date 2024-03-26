namespace a.Views;

/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView 
{
    public HomeView()
    {
        InitializeComponent();
        //LoadTreeView();
    }

    // private void LoadTreeView()
    // {
    //     treeView.Items.Clear();

    //     using (var context = new CamDataContext())
    //     {
    //         var areas = context.Areas.ToList();

    //         foreach (var area in areas)
    //         {
    //             var areaItem = new TreeViewItem
    //             {
    //                 Header = area.Name
    //             };
    //             var cams = context.Cams.Where(c => c.AreaId == area.Id).ToList();

    //             foreach (var cam in cams)
    //             {
    //                 var headerPanel = new StackPanel
    //                 {
    //                     Orientation = Orientation.Horizontal
    //                 };

    //                 var icon = new Image
    //                 {
    //                     Source = new BitmapImage(new Uri("./active.png", UriKind.Relative)), // Replace with your icon path
    //                     Width = 16,
    //                     Height = 16
    //                 };
    //                 var text = new TextBlock
    //                 {
    //                     Text = cam._ipAddress,
    //                     Margin = new Thickness(5, 0, 0, 0) 
    //                 };
    //                 headerPanel.Children.Add(icon);
    //                 headerPanel.Children.Add(text);
    //                 var camItem = new TreeViewItem
    //                 {
    //                     Header = headerPanel
    //                 };

    //                 areaItem.Items.Add(camItem);
    //             }
    //             treeView.Items.Add(areaItem);
    //         }
    //     }
    // }
}
