using System.Windows;

using a.ViewModels;
namespace a.Models;


public class CustomGridPanel : System.Windows.Controls.Panel
{
    public static readonly DependencyProperty ViewportWidthProperty =
        DependencyProperty.Register("ViewportWidth", typeof(double), typeof(CustomGridPanel), new PropertyMetadata(0.0));

    public double ViewportWidth
    {
        get { return (double)GetValue(ViewportWidthProperty); }
        set { SetValue(ViewportWidthProperty, value); }
    }

    public static readonly DependencyProperty ViewportHeightProperty =
        DependencyProperty.Register("ViewportHeight", typeof(double), typeof(CustomGridPanel), new PropertyMetadata(0.0));

    public double ViewportHeight
    {
        get { return (double)GetValue(ViewportHeightProperty); }
        set { SetValue(ViewportHeightProperty, value); }
    }
    protected override Size MeasureOverride(Size availableSize)
    {
        int columns = 4;
        double cellWidth = availableSize.Width / columns;

        foreach (UIElement child in Children)
        {
            child.Measure(new Size(cellWidth, double.PositiveInfinity));
        }

        int totalRows = (Children.Count + columns - 1) / columns;
        double totalHeight = totalRows * cellWidth; // Assuming square cells

        return new Size(availableSize.Width, totalHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        int columns = 4;
        double cellWidth = finalSize.Width / columns;
        double cellHeight = cellWidth; // Assuming square cells

        for (int i = 0; i < Children.Count; i++)
        {
            UIElement child = Children[i];
            int row = i / columns;
            int column = i % columns;

            double x = column * cellWidth;
            double y = row * cellHeight;

            child.Arrange(new Rect(x, y, cellWidth, cellHeight));

            if (child is FrameworkElement fe && fe.DataContext is CamViewModel panel)
            {
                panel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(CamViewModel.IsMaximized))
                    {
                        SetZIndex(child, panel.IsMaximized ? 1 : 0);
                        this.InvalidateMeasure();
                        this.InvalidateArrange();
                    }
                };

                if (panel.IsMaximized)
                {
                    child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
                }
                else
                {
                    child.Arrange(new Rect(x, y, cellWidth, cellHeight));
                }
            };
            
        }

        return finalSize;
    }
}

