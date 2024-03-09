﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace Slithin.Controls.Ports.RatingControl;

//ported from https://github.com/HandyOrg/HandyControl.git

/// <summary>
///     represents a rate entry
/// </summary>
public class RateItem : TemplatedControl
{
    private const string ElementIcon = "PART_Icon";
    private const string IsHalf_PseudoClass = ":ishalf";
    private const string IsSelected_PseudoClass = ":selected";
    private const string PointerEnter_PseudoClass = ":pointerenter";
    private const string PointerLeave_PseudoClass = ":pointerleave";

    /// <summary>
    ///     Defines the AllowClear property.
    /// </summary>
    public static readonly StyledProperty<bool> AllowClearProperty =
        RatingControl.AllowClearProperty.AddOwner<RateItem>();

    /// <summary>
    ///     Defines the AllowHalf property.
    /// </summary>
    public static readonly StyledProperty<bool> AllowHalfProperty =
        RatingControl.AllowHalfProperty.AddOwner<RateItem>();

    /// <summary>
    ///     Defines the Icon property.
    /// </summary>
    public static readonly StyledProperty<Geometry> IconProperty =
        RatingControl.IconProperty.AddOwner<RateItem>();

    /// <summary>
    ///     Defines the IsHalf direct property.
    /// </summary>
    public static readonly DirectProperty<RateItem, bool> IsHalfProperty =
        AvaloniaProperty.RegisterDirect<RateItem, bool>(
            nameof(IsHalf),
            o => o.IsHalf);

    /// <summary>
    ///     Defines the IsReadOnlyProperty property.
    /// </summary>
    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        RatingControl.IsReadOnlyProperty.AddOwner<RateItem>();

    /// <summary>
    ///     Defines the IsSelected property.
    /// </summary>
    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<RateItem, bool>(nameof(IsSelected));

    /// <summary>
    ///     Defines the SelectedChanged routed event.
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> SelectedChangedEvent =
        RoutedEvent.Register<RateItem, RoutedEventArgs>(nameof(SelectedChangedEvent), RoutingStrategies.Bubble);

    /// <summary>
    ///     Defines the ValueChanged routed event.
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> ValueChangedEvent =
        RoutedEvent.Register<RateItem, RoutedEventArgs>(nameof(ValueChangedEvent), RoutingStrategies.Bubble);

    /// <summary>
    ///     Defines the ClibRectangleGeometry property.
    /// </summary>
    internal static readonly StyledProperty<RectangleGeometry> ClipRectangleGeometryProperty =
        AvaloniaProperty.Register<RateItem, RectangleGeometry>(nameof(ClipRectangleGeometry)
            , new(new(0, 0, 20, 20)));

    /// <summary>
    ///     Defines the HalfWidth property.
    /// </summary>
    internal static readonly StyledProperty<double> HalfWidthProperty =
        AvaloniaProperty.Register<RateItem, double>(nameof(HalfWidth));

    private static readonly List<string> PointerPseudoClasses = new()
    {
        PointerEnter_PseudoClass, PointerLeave_PseudoClass
    };

    private Layoutable _icon;

    private bool _isHalf;
    private bool _isLoaded;

    private bool _isMouseLeftButtonDown;

    private bool _isSentValue;

    /// <summary>
    ///     registers some changed handlers
    /// </summary>
    public RateItem()
    {
        AllowHalfProperty.Changed.AddClassHandler<RateItem>((o, e) => OnAllowHalfChanged(o, e));
        IsSelectedProperty.Changed.AddClassHandler<RateItem>((o, e) => OnIsSelectedChanged(o, e));
        WidthProperty.Changed.AddClassHandler<RateItem>((o, e) => OnSizeChanged(o, e));
        HeightProperty.Changed.AddClassHandler<RateItem>((o, e) => OnSizeChanged(o, e));
    }

    /// <summary>
    ///     Gets or sets AllowClear.
    /// </summary>
    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    /// <summary>
    ///     Gets or sets AllowHalf.
    /// </summary>
    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
    }

    /// <summary>
    ///     Gets or sets Icon.
    /// </summary>
    public Geometry Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    ///     Gets or sets IsHalf.
    /// </summary>
    public bool IsHalf
    {
        get => _isHalf;
        set
        {
            SetAndRaise(IsHalfProperty, ref _isHalf, value);

            PseudoClasses.Set(IsHalf_PseudoClass, _isHalf);
        }
    }

    /// <summary>
    ///     Gets or sets IsReadOnly.
    /// </summary>
    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    ///     Gets or sets IsSelected.
    /// </summary>

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    /// <summary>
    ///     Gets or sets ClibRectangleGeometry.
    /// </summary>
    internal RectangleGeometry ClipRectangleGeometry
    {
        get => GetValue(ClipRectangleGeometryProperty);
        set => SetValue(ClipRectangleGeometryProperty, value);
    }

    /// <summary>
    ///     Gets or sets HalfWidth.
    /// </summary>
    internal double HalfWidth
    {
        get => GetValue(HalfWidthProperty);
        set => SetValue(HalfWidthProperty, value);
    }

    internal int Index { get; set; }

    /// <summary>
    ///     Gets or sets SelectedChanged eventhandler.
    /// </summary>
    public event EventHandler SelectedChanged
    {
        add => AddHandler(SelectedChangedEvent, value);
        remove => RemoveHandler(SelectedChangedEvent, value);
    }

    /// <summary>
    ///     Gets or sets ValueChanged eventhandler.
    /// </summary>
    public event EventHandler ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    /// <summary>
    ///     Creates a RateItem with values from the <see cref="RatingControl" />
    ///     and sets some Bindings
    /// </summary>
    internal static RateItem Create(RatingControl ratingControl, int i)
    {
        var rateItem = new RateItem();
        rateItem.Index = i;
        rateItem.Width = ratingControl.ItemWidth;
        rateItem.Height = ratingControl.ItemHeight;
        rateItem.Margin = ratingControl.ItemMargin;
        rateItem.AllowHalf = ratingControl.AllowHalf;
        rateItem.AllowClear = ratingControl.AllowClear;
        rateItem.Icon = ratingControl.Icon;
        rateItem.IsReadOnly = ratingControl.IsReadOnly;

        var binding = new Binding();
        binding.Source = ratingControl;
        binding.Path = BackgroundProperty.Name;
        rateItem.Bind(BackgroundProperty, binding);

        binding = new();
        binding.Source = ratingControl;
        binding.Path = ForegroundProperty.Name;
        rateItem.Bind(ForegroundProperty, binding);

        return rateItem;
    }

    /// <summary>
    ///     resolves the controls
    /// </summary>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _icon = e.NameScope.Find<Layoutable>(ElementIcon);

        if (_isLoaded)
        {
            if (_icon == null)
            {
                return;
            }

            _icon.IsVisible = IsSelected;
        }
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        PointerPseudoClasses.ForEach(item => PseudoClasses.Remove(item));

        PseudoClasses.Add(PointerEnter_PseudoClass);

        base.OnPointerEntered(e);
        if (IsReadOnly)
        {
            return;
        }

        _isSentValue = false;

        IsSelected = true;
        var p = e.GetPosition(this);
        IsHalf = p.X < Width / 2;

        RaiseEvent(new(SelectedChangedEvent) {Source = this});
    }

    /// <summary>
    ///     sets pointerleave pseudo class
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPointerExited(PointerEventArgs e)
    {
        PointerPseudoClasses.ForEach(item => PseudoClasses.Remove(item));

        PseudoClasses.Add(PointerLeave_PseudoClass);

        base.OnPointerExited(e);
        if (IsReadOnly)
        {
            return;
        }

        _isMouseLeftButtonDown = false;
    }

    /// <summary>
    ///     handles left button click
    /// </summary>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var properties = e.GetCurrentPoint(this).Properties;

        if (properties.IsLeftButtonPressed)
        {
            _isMouseLeftButtonDown = true;

            if (IsReadOnly)
            {
                return;
            }

            if (Index == 1 && AllowClear)
            {
                if (IsSelected)
                {
                    if (!_isSentValue)
                    {
                        RaiseEvent(new(ValueChangedEvent) {Source = this});
                        _isMouseLeftButtonDown = false;
                        _isSentValue = true;
                        return;
                    }

                    _isSentValue = false;
                    IsSelected = false;
                    IsHalf = false;
                }
                else
                {
                    IsSelected = true;
                    if (AllowHalf)
                    {
                        var p = e.GetPosition(this);
                        IsHalf = p.X < Width / 2;
                    }
                }
            }

            RaiseEvent(new(ValueChangedEvent) {Source = this});
            _isMouseLeftButtonDown = false;
        }
    }

    /// <summary>
    ///     if true PointerMoved event is registered else unregistered
    /// </summary>
    /// <param name="handle"></param>
    private void HandlePointerMovedEvent(bool handle)
    {
        if (handle)
        {
            PointerMoved += RateItem_PointerMoved;
        }
        else
        {
            PointerMoved -= RateItem_PointerMoved;
        }
    }

    /// <summary>
    ///     add or remove the PointerMoved event
    /// </summary>
    private void OnAllowHalfChanged(RateItem rateItem, AvaloniaPropertyChangedEventArgs e)
    {
        rateItem.HandlePointerMovedEvent((bool)e.NewValue);
    }

    /// <summary>
    ///     updates the selection of the items
    /// </summary>
    private void OnIsSelectedChanged(RateItem rateItem, AvaloniaPropertyChangedEventArgs e)
    {
        if (rateItem._icon == null)
        {
            return;
        }

        var isSelected = (bool)e.NewValue;

        rateItem.PseudoClasses.Set(IsSelected_PseudoClass, isSelected);
        rateItem._icon.IsVisible = isSelected;
    }

    /// <summary>
    ///     updates <see cref="HalfWidth" /> and <see cref="ClipRectangleGeometry" />
    /// </summary>
    private void OnSizeChanged(RateItem o, AvaloniaPropertyChangedEventArgs e)
    {
        HalfWidth = Width / 2;

        ClipRectangleGeometry = new(new(0, 0, HalfWidth, Height));
    }

    /// <summary>
    ///     updates <see cref="IsHalf" />
    /// </summary>
    private void RateItem_PointerMoved(object sender, PointerEventArgs e)
    {
        if (IsReadOnly)
        {
            return;
        }

        if (!AllowHalf)
        {
            return;
        }

        var p = e.GetPosition(this);
        IsHalf = p.X < Width / 2;
    }
}
