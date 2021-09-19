using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Slithin.Models;

namespace Slithin.Controls
{
    public class StoreCardCollection : TemplatedControl
    {
        public static StyledProperty<ObservableCollection<Sharable>> CardsProperty =
            AvaloniaProperty.Register<StoreCardCollection, ObservableCollection<Sharable>>("Cards");
        public static StyledProperty<string> CategoryProperty =
            AvaloniaProperty.Register<StoreCardCollection, string>("Category");
        public static StyledProperty<ICommand> InstallCommandProperty =
            AvaloniaProperty.Register<StoreCardCollection, ICommand>("InstallCommand");
        public static StyledProperty<ICommand> MoreCommandProperty =
            AvaloniaProperty.Register<StoreCardCollection, ICommand>("MoreCommand");
        public static StyledProperty<Sharable> SelectedCardProperty =
            AvaloniaProperty.Register<StoreCardCollection, Sharable>("SelectedCard");
        public static StyledProperty<ICommand> UninstallCommandProperty =
            AvaloniaProperty.Register<StoreCardCollection, ICommand>("UninstallCommand");

        public ObservableCollection<Sharable> Cards
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

        public Sharable SelectedCard
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
}
