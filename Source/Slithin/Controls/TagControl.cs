using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Slithin.Controls;

public class TagControl : TemplatedControl
{
    public static StyledProperty<ICommand> RemoveCommandProperty =
        AvaloniaProperty.Register<StoreCardCollection, ICommand>("RemoveCommand");

    public static StyledProperty<string> TagProperty =
            AvaloniaProperty.Register<StoreCardCollection, string>("Tag");

    public ICommand RemoveCommand
    {
        get => GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    public string Tag
    {
        get => GetValue(TagProperty);
        set => SetValue(TagProperty, value);
    }
}
