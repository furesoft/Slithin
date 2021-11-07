using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Slithin.Controls
{
    public class ContentDialog : ContentControl, IStyleable
    {
        public static StyledProperty<object> DialogContentProperty =
            AvaloniaProperty.Register<ContentDialog, object>("DialogContent");

        public static StyledProperty<bool> IsOpenedProperty =
            AvaloniaProperty.Register<ContentDialog, bool>("IsOpened");

        public object DialogContent
        {
            get => GetValue(DialogContentProperty); set => SetValue(DialogContentProperty, value);
        }

        public bool IsOpened
        {
            get => GetValue(IsOpenedProperty);
            set => SetValue(IsOpenedProperty, value);
        }

        Type IStyleable.StyleKey => typeof(ContentDialog);
    }
}
