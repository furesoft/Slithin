using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Slithin.Controls.Ports.ResizeRotateControl;

//ported from https://www.codeproject.com/Articles/22952/WPF-Diagram-Designer-Part-1

/// <summary>
/// rounds the double value with Math.Round
/// </summary>
public class DoubleFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var d = (double)value;
        return Math.Round(d);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}
