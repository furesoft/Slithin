using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Slithin.Controls;

public class GalleryControl : ItemsControl
{
    public static readonly StyledProperty<bool> AreButtonsVisibleProperty =
        AvaloniaProperty.Register<GalleryControl, bool>(nameof(AreButtonsVisible), true);

    public static readonly StyledProperty<ObservableCollection<Indicator>> IndicatorsProperty =
        AvaloniaProperty.Register<GalleryControl, ObservableCollection<Indicator>>(nameof(Indicators),
            new());

    public bool AreButtonsVisible
    {
        get => GetValue(AreButtonsVisibleProperty);
        set => SetValue(AreButtonsVisibleProperty, value);
    }

    public ObservableCollection<Indicator> Indicators
    {
        get => GetValue(IndicatorsProperty);
        set => SetValue(IndicatorsProperty, value);
    }

    void ItemsCollectionChanged(object sender)
    {
        Indicators.Add(new Indicator());
    }

    public GalleryControl()
    {
        ItemsSourceProperty.Changed.Subscribe(ItemsCollectionChanged);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var partLeft = e.NameScope.Find<Button>("PART_LEFT");
        var partCarousel = e.NameScope.Find<Carousel>("PART_CAROUSEL");
        var partRight = e.NameScope.Find<Button>("PART_RIGHT");

        var timer = new System.Timers.Timer();

        async void OnTimerOnElapsed(object s, ElapsedEventArgs e)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (partCarousel.SelectedIndex < partCarousel.ItemCount - 1)
                {
                    partCarousel.SelectedIndex++;
                }
                else
                {
                    timer.Stop();
                    timer.Dispose();
                }

                partRight.IsEnabled = partCarousel.SelectedIndex < partCarousel.ItemCount - 1;
                partLeft.IsEnabled = true;
            });
        }

        timer.Elapsed += OnTimerOnElapsed;
        timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;

        void OnPartLeftOnClick(object o, RoutedEventArgs routedEventArgs)
        {
            if (partCarousel.SelectedIndex != 0)
            {
                partCarousel.SelectedIndex--;
            }

            partLeft.IsEnabled = partCarousel.SelectedIndex > 0;
            partRight.IsEnabled = true;
        }

        partLeft.Click += OnPartLeftOnClick;

        void OnPartRightOnClick(object o, RoutedEventArgs routedEventArgs)
        {
            if (partCarousel.SelectedIndex < partCarousel.ItemCount - 1)
            {
                partCarousel.SelectedIndex++;
            }

            partRight.IsEnabled = partCarousel.SelectedIndex < partCarousel.ItemCount - 1;
            partLeft.IsEnabled = true;
        }

        partRight.Click += OnPartRightOnClick;

        timer.Start();
    }
}

public class Indicator
{
}
