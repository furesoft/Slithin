using Avalonia;
using Avalonia.Controls;

namespace Slithin.Controls.Ports.Panels;

public class SimplePanel : Panel
{
    /// <summary>
    /// arranges the children
    /// </summary>
    /// <param name="arrangeSize"></param>
    /// <returns></returns>
    protected override Size ArrangeOverride(Size arrangeSize)
    {
        foreach (Control child in Children)
        {
            child?.Arrange(new Rect(arrangeSize));
        }

        return arrangeSize;
    }

    /// <summary>
    /// calculates the size by its children
    /// </summary>
    /// <param name="availableSize"></param>
    /// <returns></returns>
    protected override Size MeasureOverride(Size availableSize)
    {
        var maxSize = new Size();

        foreach (Control child in Children)
        {
            if (child != null)
            {
                child.Measure(availableSize);
                double width = Math.Max(maxSize.Width, child.DesiredSize.Width);
                double height = Math.Max(maxSize.Height, child.DesiredSize.Height);
                maxSize = new Size(width, height);
            }
        }

        return maxSize;
    }
}
