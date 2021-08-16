using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Slithin.Controls
{
    public class GalleryControl : TemplatedControl
    {
        public static StyledProperty<ObservableCollection<IImage>> ImagesProperty =
            AvaloniaProperty.Register<GalleryControl, ObservableCollection<IImage>>(nameof(Images));

        public ObservableCollection<IImage> Images
        {
            get { return GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var part_left = e.NameScope.Find<Button>("PART_LEFT");
            var part_carousel = e.NameScope.Find<Carousel>("PART_CAROUSEL");
            var part_right = e.NameScope.Find<Button>("PART_RIGHT");

            part_left.Click += (s, e) =>
            {
                if (part_carousel.SelectedIndex != 0)
                {
                    part_carousel.SelectedIndex--;
                }

                part_left.IsEnabled = part_carousel.SelectedIndex > 0;
                part_right.IsEnabled = true;
            };
            part_right.Click += (s, e) =>
            {
                if (part_carousel.SelectedIndex < part_carousel.ItemCount - 1)
                {
                    part_carousel.SelectedIndex++;
                }

                part_right.IsEnabled = part_carousel.SelectedIndex < part_carousel.ItemCount - 1;
                part_left.IsEnabled = true;
            };
        }
    }
}
