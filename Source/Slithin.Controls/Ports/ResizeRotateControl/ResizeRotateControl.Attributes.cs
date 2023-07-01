﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Metadata;

namespace Slithin.Controls.Ports.ResizeRotateControl;

public partial class ResizeRotateControl
{
    /// <summary>
    /// Defines the <see cref="AllowDragOutOfView"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> AllowDragOutOfViewProperty =
        AvaloniaProperty.Register<ResizeRotateControl, bool>(nameof(AllowDragOutOfView), defaultValue: false);

    /// <summary>
    /// Defines the <see cref="BouncedControl"/> property.
    /// </summary>
    public static readonly StyledProperty<Control> BouncedControlProperty =
        AvaloniaProperty.Register<ResizeRotateControl, Control>(nameof(BouncedControl));

    /// <summary>
    /// Defines the CanDrag property.
    /// </summary>
    public static readonly StyledProperty<bool> CanDragProperty =
    AvaloniaProperty.Register<ResizeRotateControl, bool>(nameof(CanDrag), defaultValue: true);

    /// <summary>
    /// Defines the CanResize property.
    /// </summary>
    public static readonly StyledProperty<bool> CanResizeProperty =
    AvaloniaProperty.Register<ResizeRotateControl, bool>(nameof(CanResize), defaultValue: true);

    /// <summary>
    /// Defines the Content property.
    /// </summary>
    public static readonly StyledProperty<object> ContentProperty =
    AvaloniaProperty.Register<ResizeRotateControl, object>(nameof(Content));

    /// <summary>
    /// Defines the IsRotationEnabled property.
    /// </summary>
    public static readonly StyledProperty<bool> IsRotationEnabledProperty =
    AvaloniaProperty.Register<ResizeRotateControl, bool>(nameof(IsRotationEnabled), defaultValue: true);

    /// <summary>
    /// Defines the Moved routed event.
    /// </summary>
    public static readonly RoutedEvent<VectorEventArgs> MovedEvent =
    RoutedEvent.Register<ResizeRotateControl, VectorEventArgs>(nameof(MovedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Defines the OuterRectBottom property.
    /// </summary>
    public static readonly StyledProperty<double> OuterRectBottomProperty =
    AvaloniaProperty.Register<ResizeRotateControl, double>(nameof(OuterRectBottom));

    /// <summary>
    /// Defines the OuterRectLeft property.
    /// </summary>
    public static readonly StyledProperty<double> OuterRectLeftProperty =
    AvaloniaProperty.Register<ResizeRotateControl, double>(nameof(OuterRectLeft));

    /// <summary>
    /// Defines the OuterRectRight property.
    /// </summary>
    public static readonly StyledProperty<double> OuterRectRightProperty =
    AvaloniaProperty.Register<ResizeRotateControl, double>(nameof(OuterRectRight));

    /// <summary>
    /// Defines the OuterRectStrokeBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush> OuterRectStrokeBrushProperty =
    AvaloniaProperty.Register<ResizeRotateControl, IBrush>(nameof(OuterRectStrokeBrush));

    /// <summary>
    /// Defines the OuterRectStrokeThickness property.
    /// </summary>
    public static readonly StyledProperty<double> OuterRectStrokeThicknessProperty =
    AvaloniaProperty.Register<ResizeRotateControl, double>(nameof(OuterRectStrokeThickness));

    /// <summary>
    /// Defines the OuterRectTop property.
    /// </summary>
    public static readonly StyledProperty<double> OuterRectTopProperty =
    AvaloniaProperty.Register<ResizeRotateControl, double>(nameof(OuterRectTop));

    /// <summary>
    /// Defines the PositionChanged routed event.
    /// </summary>
    public static readonly RoutedEvent<PositionChangedEventArgs> PositionChangedEvent =
    RoutedEvent.Register<ResizeRotateControl, PositionChangedEventArgs>(nameof(PositionChangedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Defines the ResizedEvent routed event.
    /// </summary>
    public static readonly RoutedEvent<VectorEventArgs> ResizedEvent =
    RoutedEvent.Register<ResizeRotateControl, VectorEventArgs>(nameof(ResizedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Defines the Rotated routed event.
    /// </summary>
    public static readonly RoutedEvent<RotatedEventArgs> RotatedEvent =
    RoutedEvent.Register<ResizeRotateControl, RotatedEventArgs>
                (nameof(RotatedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Defines the RotationAngle property.
    /// </summary>
    public static readonly StyledProperty<double> RotationAngleProperty =
    AvaloniaProperty.Register<ResizeRotateControl, double>(nameof(RotationAngle));

    /// <summary>
    /// Defines the ShowOuterRect property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowOuterRectProperty =
    AvaloniaProperty.Register<ResizeRotateControl, bool>(nameof(ShowOuterRect), defaultValue: true);

    /// <summary>
    /// Defines the SizeChromeTextBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush> SizeChromeTextBrushProperty =
    AvaloniaProperty.Register<ResizeRotateControl, IBrush>(nameof(SizeChromeTextBrush));

    /// <summary>
    /// Defines the ThumbFillBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush> ThumbFillBrushProperty =
    AvaloniaProperty.Register<ResizeRotateControl, IBrush>(nameof(ThumbFillBrush));

    /// <summary>
    /// Defines the ThumbStrokeBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush> ThumbStrokeBrushProperty =
    AvaloniaProperty.Register<ResizeRotateControl, IBrush>(nameof(ThumbStrokeBrush));

    private static readonly string PART_Content = "PART_Content";
    private static readonly string PART_ContentGrid = "PART_ContentGrid";
    private static readonly string PART_MoveThumb = "PART_MoveThumb";
    private static readonly string PART_ResizeThumbBottomCenter = "PART_ResizeThumbBottomCenter";
    private static readonly string PART_ResizeThumbBottomLeft = "PART_ResizeThumbBottomLeft";
    private static readonly string PART_ResizeThumbBottomRight = "PART_ResizeThumbBottomRight";
    private static readonly string PART_ResizeThumbLeftCenter = "PART_ResizeThumbLeftCenter";
    private static readonly string PART_ResizeThumbRightCenter = "PART_ResizeThumbRightCenter";
    private static readonly string PART_ResizeThumbTopCenter = "PART_ResizeThumbTopCenter";
    private static readonly string PART_ResizeThumbTopLeft = "PART_ResizeThumbTopLeft";
    private static readonly string PART_ResizeThumbTopRight = "PART_ResizeThumbTopRight";
    private static readonly string PART_RotateThumb = "PART_RotateThumb";
    private static readonly string PART_SizeChrome = "PART_SizeChrome";
    private static readonly string PART_ThumbGrid = "PART_ThumbGrid";
    private static readonly string PART_VisualGrid = "PART_VisualGrid";
    private ContentControl _contentControl;
    private Grid _contentGrid;
    private Thickness _contentGridMargin;
    private MoveThumb _moveThumb;
    private List<ResizeThumb> _resizeThumbs = new List<ResizeThumb>();
    private RotateThumb _rotateThumb;
    private Grid _thumbGrid;
    private Thickness _thumbGridMargin;
    private Grid _visualGrid;
    private Thickness _visualGridMargin;

    /// <summary>
    /// Gets or sets Moved eventhandler.
    /// </summary>
    public event EventHandler<VectorEventArgs> Moved
    {
        add
        {
            AddHandler(MovedEvent, value);
        }
        remove
        {
            RemoveHandler(MovedEvent, value);
        }
    }

    /// <summary>
    /// Gets or sets PositionChanged eventhandler.
    /// </summary>
    public event EventHandler<PositionChangedEventArgs> PositionChanged
    {
        add
        {
            AddHandler(PositionChangedEvent, value);
        }
        remove
        {
            RemoveHandler(PositionChangedEvent, value);
        }
    }

    /// <summary>
    /// Gets or sets ResizedEvent eventhandler.
    /// </summary>
    public event EventHandler<VectorEventArgs> Resized
    {
        add
        {
            AddHandler(ResizedEvent, value);
        }
        remove
        {
            RemoveHandler(ResizedEvent, value);
        }
    }

    /// <summary>
    /// Gets or sets Rotated eventhandler.
    /// </summary>
    public event EventHandler<RotatedEventArgs> Rotated
    {
        add
        {
            AddHandler(RotatedEvent, value);
        }
        remove
        {
            RemoveHandler(RotatedEvent, value);
        }
    }

    /// <summary>
    /// Gets or sets AllowDragOutOfView.
    /// </summary>
    public bool AllowDragOutOfView
    {
        get { return (bool)GetValue(AllowDragOutOfViewProperty); }
        set { SetValue(AllowDragOutOfViewProperty, value); }
    }

    /// <summary>
    /// Gets or sets BouncedControl.
    /// </summary>
    public Control BouncedControl
    {
        get { return (Control)GetValue(BouncedControlProperty); }
        set { SetValue(BouncedControlProperty, value); }
    }

    /// <summary>
    /// Gets or sets CanDrag.
    /// </summary>
    public bool CanDrag
    {
        get { return (bool)GetValue(CanDragProperty); }
        set { SetValue(CanDragProperty, value); }
    }

    /// <summary>
    /// Gets or sets CanResize.
    /// </summary>
    public bool CanResize
    {
        get { return (bool)GetValue(CanResizeProperty); }
        set { SetValue(CanResizeProperty, value); }
    }

    /// <summary>
    /// Gets or sets Content.
    /// </summary>
    [Content]
    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    /// <summary>
    /// Gets or sets IsRotationEnabled.
    /// </summary>
    public bool IsRotationEnabled
    {
        get { return (bool)GetValue(IsRotationEnabledProperty); }
        set { SetValue(IsRotationEnabledProperty, value); }
    }

    /// <summary>
    /// Gets or sets OuterRectBottom.
    /// </summary>
    public double OuterRectBottom
    {
        get { return (double)GetValue(OuterRectBottomProperty); }
        private set { SetValue(OuterRectBottomProperty, value); }
    }

    /// <summary>
    /// Gets or sets OuterRectLeft.
    /// </summary>
    public double OuterRectLeft
    {
        get { return (double)GetValue(OuterRectLeftProperty); }
        private set { SetValue(OuterRectLeftProperty, value); }
    }

    /// <summary>
    /// Gets or sets OuterRectRight.
    /// </summary>
    public double OuterRectRight
    {
        get { return (double)GetValue(OuterRectRightProperty); }
        private set { SetValue(OuterRectRightProperty, value); }
    }

    /// <summary>
    /// Gets or sets OuterRectStrokeBrush.
    /// </summary>
    public IBrush OuterRectStrokeBrush
    {
        get { return (IBrush)GetValue(OuterRectStrokeBrushProperty); }
        set { SetValue(OuterRectStrokeBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets OuterRectStrokeThickness.
    /// </summary>
    public double OuterRectStrokeThickness
    {
        get { return (double)GetValue(OuterRectStrokeThicknessProperty); }
        set { SetValue(OuterRectStrokeThicknessProperty, value); }
    }

    /// <summary>
    /// Gets or sets OuterRectTop.
    /// </summary>
    public double OuterRectTop
    {
        get { return (double)GetValue(OuterRectTopProperty); }
        private set { SetValue(OuterRectTopProperty, value); }
    }

    /// <summary>
    /// Gets or sets RotationAngle.
    /// </summary>
    public double RotationAngle
    {
        get { return (double)GetValue(RotationAngleProperty); }
        private set { SetValue(RotationAngleProperty, value); }
    }

    /// <summary>
    /// Gets or sets ShowOuterRect.
    /// </summary>
    public bool ShowOuterRect
    {
        get { return (bool)GetValue(ShowOuterRectProperty); }
        set { SetValue(ShowOuterRectProperty, value); }
    }

    /// <summary>
    /// Gets or sets SizeChromeTextBrush.
    /// </summary>
    public IBrush SizeChromeTextBrush
    {
        get { return (IBrush)GetValue(SizeChromeTextBrushProperty); }
        set { SetValue(SizeChromeTextBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets ThumbFillBrush.
    /// </summary>
    public IBrush ThumbFillBrush
    {
        get { return (IBrush)GetValue(ThumbFillBrushProperty); }
        set { SetValue(ThumbFillBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets ThumbStrokeBrush.
    /// </summary>
    public IBrush ThumbStrokeBrush
    {
        get { return (IBrush)GetValue(ThumbStrokeBrushProperty); }
        set { SetValue(ThumbStrokeBrushProperty, value); }
    }
}
