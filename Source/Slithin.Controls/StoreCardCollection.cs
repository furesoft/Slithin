using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Slithin.Controls;

public class StoreCardCollection : TemplatedControl
{
    public static readonly StyledProperty<ObservableCollection<object>> CardsProperty =
        AvaloniaProperty.Register<StoreCardCollection, ObservableCollection<object>>("Cards");

    public static readonly StyledProperty<string> CategoryProperty =
        AvaloniaProperty.Register<StoreCardCollection, string>("Category");

    public static readonly StyledProperty<ICommand> InstallCommandProperty =
        AvaloniaProperty.Register<StoreCardCollection, ICommand>("InstallCommand");

    public static readonly StyledProperty<ICommand> MoreCommandProperty =
        AvaloniaProperty.Register<StoreCardCollection, ICommand>("MoreCommand");

    public static readonly StyledProperty<string> MoreTitleProperty =
                   AvaloniaProperty.Register<StoreCardCollection, string>("MoreTitle");

    public static readonly StyledProperty<object> SelectedCardProperty =
        AvaloniaProperty.Register<StoreCardCollection, object>("SelectedCard");

    public static readonly StyledProperty<ICommand> UninstallCommandProperty =
        AvaloniaProperty.Register<StoreCardCollection, ICommand>("UninstallCommand");

    public ObservableCollection<object> Cards
    {
        get => GetValue(CardsProperty);
        set => SetValue(CardsProperty, value);
    }

    public string Category
    {
        get => GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public ICommand InstallCommand
    {
        get => GetValue(InstallCommandProperty);
        set => SetValue(InstallCommandProperty, value);
    }

    public ICommand MoreCommand
    {
        get => GetValue(MoreCommandProperty);
        set => SetValue(MoreCommandProperty, value);
    }

    public string MoreTitle
    {
        get => GetValue(MoreTitleProperty);
        set => SetValue(MoreTitleProperty, value);
    }

    public object SelectedCard
    {
        get => GetValue(SelectedCardProperty);
        set => SetValue(SelectedCardProperty, value);
    }

    public ICommand UninstallCommand
    {
        get => GetValue(UninstallCommandProperty);
        set => SetValue(UninstallCommandProperty, value);
    }
}
