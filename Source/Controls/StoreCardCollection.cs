using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Slithin.Models;

namespace Slithin.Controls
{
    public class StoreCardCollection : TemplatedControl
    {
        public static StyledProperty<ObservableCollection<Sharable>> CardsProperty = AvaloniaProperty.Register<StoreCardCollection, ObservableCollection<Sharable>>("Cards");
        public static StyledProperty<string> CategoryProperty = AvaloniaProperty.Register<StoreCardCollection, string>("Category");
        public static StyledProperty<Sharable> SelectedCardProperty = AvaloniaProperty.Register<StoreCardCollection, Sharable>("SelectedCard");

        public ObservableCollection<Sharable> Cards
        {
            get { return GetValue(CardsProperty); }
            set { SetValue(CardsProperty, value); }
        }

        public string Category
        {
            get { return GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public Sharable SelectedCard
        {
            get { return GetValue(SelectedCardProperty); }
            set { SetValue(SelectedCardProperty, value); }
        }
    }
}
