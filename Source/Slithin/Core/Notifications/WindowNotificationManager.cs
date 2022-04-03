using System;
using System.Collections;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Rendering;
using Avalonia.VisualTree;

namespace Slithin.Core.Notifications;

/// <summary>
/// An <see cref="INotificationManager"/> that displays notifications in a <see cref="Window"/>.
/// </summary>
[PseudoClasses(":topleft", ":topright", ":bottomleft", ":bottomright")]
public class WindowNotificationManager : TemplatedControl, IManagedNotificationManager, ICustomSimpleHitTest
{
    /// <summary>
    /// Defines the <see cref="MaxItems"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MaxItemsProperty =
      AvaloniaProperty.Register<WindowNotificationManager, int>(nameof(MaxItems), 5);

    /// <summary>
    /// Defines the <see cref="Position"/> property.
    /// </summary>
    public static readonly StyledProperty<NotificationPosition> PositionProperty =
      AvaloniaProperty.Register<WindowNotificationManager, NotificationPosition>(nameof(Position), Avalonia.Controls.Notifications.NotificationPosition.TopRight);

    private IList? _items;

    static WindowNotificationManager()
    {
        HorizontalAlignmentProperty.OverrideDefaultValue<WindowNotificationManager>(Avalonia.Layout.HorizontalAlignment.Stretch);
        VerticalAlignmentProperty.OverrideDefaultValue<WindowNotificationManager>(Avalonia.Layout.VerticalAlignment.Stretch);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowNotificationManager"/> class.
    /// </summary>
    /// <param name="host">The window that will host the control.</param>
    public WindowNotificationManager(Window host)
    {
        if (VisualChildren.Count != 0)
        {
            Install(host);
        }
        else
        {
            Observable.FromEventPattern<TemplateAppliedEventArgs>(host, nameof(host.TemplateApplied)).Take(1)
                .Subscribe(_ =>
                {
                    Install(host);
                });
        }

        UpdatePseudoClasses(Position);
    }

    /// <summary>
    /// Defines the maximum number of notifications visible at once.
    /// </summary>
    public int MaxItems
    {
        get { return GetValue(MaxItemsProperty); }
        set { SetValue(MaxItemsProperty, value); }
    }

    /// <summary>
    /// Defines which corner of the screen notifications can be displayed in.
    /// </summary>
    /// <seealso cref="NotificationPosition"/>
    public NotificationPosition Position
    {
        get { return GetValue(PositionProperty); }
        set { SetValue(PositionProperty, value); }
    }

    public bool HitTest(Point point) => VisualChildren.HitTestCustom(point);

    /// <inheritdoc/>
    public Task<NotificationCard> Show(INotification content)
    {
        return Show(content as object);
    }

    public Task<NotificationCard> Show(Control control, TimeSpan expiration, string title)
    {
        var notification = new Notification(title, control, NotificationType.Information, expiration);

        return Show(notification);
    }

    /// <inheritdoc/>
    public async Task<NotificationCard> Show(object content)
    {
        var notification = content as INotification;

        var notificationControl = new NotificationCard
        {
            Content = content
        };

        if (notification != null)
        {
            notification.Control = notificationControl;

            notificationControl.NotificationClosed += (sender, args) =>
            {
                notification.OnClose?.Invoke();

                _items?.Remove(sender);
            };
        }

        notificationControl.PointerPressed += (sender, args) =>
        {
            if (notification != null && notification.OnClick != null)
            {
                notification.OnClick.Invoke();
            }

            (sender as NotificationCard)?.Close();
        };

        _items?.Add(notificationControl);

        if (_items?.OfType<NotificationCard>().Count(i => !i.IsClosing) > MaxItems)
        {
            _items.OfType<NotificationCard>().First(i => !i.IsClosing).Close();
        }

        if (notification != null && notification.Expiration == TimeSpan.Zero)
        {
            return notificationControl;
        }

        await Task.Delay(notification?.Expiration ?? TimeSpan.FromSeconds(5));

        notificationControl.Close();

        return null;
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        var itemsControl = e.NameScope.Find<Panel>("PART_Items");
        _items = itemsControl?.Children;
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PositionProperty)
        {
            UpdatePseudoClasses(change.NewValue.GetValueOrDefault<NotificationPosition>());
        }
    }

    /// <summary>
    /// Installs the <see cref="WindowNotificationManager"/> within the <see cref="AdornerLayer"/>
    /// of the host <see cref="Window"/>.
    /// </summary>
    /// <param name="host">The <see cref="Window"/> that will be the host.</param>
    private void Install(Window host)
    {
        var adornerLayer = host.FindDescendantOfType<VisualLayerManager>()?.AdornerLayer;

        adornerLayer?.Children.Add(this);
    }

    private void UpdatePseudoClasses(NotificationPosition position)
    {
        PseudoClasses.Set(":topleft", position == Avalonia.Controls.Notifications.NotificationPosition.TopLeft);
        PseudoClasses.Set(":topright", position == Avalonia.Controls.Notifications.NotificationPosition.TopRight);
        PseudoClasses.Set(":bottomleft", position == Avalonia.Controls.Notifications.NotificationPosition.BottomLeft);
        PseudoClasses.Set(":bottomright", position == Avalonia.Controls.Notifications.NotificationPosition.BottomRight);
    }
}
