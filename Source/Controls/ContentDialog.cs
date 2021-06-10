using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using System;

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
            get { return GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        public bool IsOpened
        {
            get { return GetValue<bool>(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }

        Type IStyleable.StyleKey => typeof(ContentDialog);
    }
}