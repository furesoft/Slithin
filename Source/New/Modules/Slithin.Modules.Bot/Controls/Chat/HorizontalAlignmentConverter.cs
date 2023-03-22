using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Bot.Controls.Chat;

public class HorizontalAlignmentConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new HorizontalAlignmentConverter();
    }
}
