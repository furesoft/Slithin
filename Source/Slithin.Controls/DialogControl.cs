using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Slithin.Controls;

public class DialogControl : ContentControl, IStyleable
{
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<DialogControl, ICommand>(nameof(Command));

    public static readonly StyledProperty<string> CommandTextProperty =
        AvaloniaProperty.Register<DialogControl, string>(nameof(CommandText));

    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<DialogControl, string>(nameof(Header));

    public static readonly StyledProperty<bool> IsCancelEnabledProperty =
        AvaloniaProperty.Register<DialogControl, bool>(nameof(IsCancelEnabled));

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public string CommandText
    {
        get => GetValue(CommandTextProperty);
        set => SetValue(CommandTextProperty, value);
    }

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public bool IsCancelEnabled
    {
        get => GetValue(IsCancelEnabledProperty);
        set => SetValue(IsCancelEnabledProperty, value);
    }

    Type IStyleable.StyleKey => typeof(DialogControl);
}
