using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Slithin.Controls.Ports.Extensions;

namespace Slithin.Controls.Ports.ResizeRotateControl;

//ported from https://www.codeproject.com/Articles/22952/WPF-Diagram-Designer-Part-1

/// <summary>
/// thumb for the rotation
/// </summary>
public class RotateThumb : Thumb
{
    /// <summary>
    /// Defines the FillBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush> FillBrushProperty =
    AvaloniaProperty.Register<RotateThumb, IBrush>(nameof(FillBrush));

    /// <summary>
    /// Defines the RotateFinsished routed event.
    /// </summary>
    public static readonly RoutedEvent<RotatedEventArgs> RotateFinsishedEvent =
    RoutedEvent.Register<RotateThumb, RotatedEventArgs>
                (nameof(RotateFinsishedEvent), RoutingStrategies.Bubble);

    /// <summary>
    /// Defines the StrokeBrush property.
    /// </summary>
    public static readonly StyledProperty<IBrush> StrokeBrushProperty =
    AvaloniaProperty.Register<RotateThumb, IBrush>(nameof(StrokeBrush));

    private Canvas _canvas;

    private Point _centerPoint;

    private Point _currentPoint;

    private double _currentRotateAngle;

    private Control _designerItem;

    private double _initialAngle;

    private RotateTransform _rotateTransform;

    private Vector _startVector;

    public RotateThumb()
    {
        DragDelta += RotateThumb_DragDelta;
        DragStarted += RotateThumb_DragStarted;
        DragCompleted += RotateThumb_DragCompleted;
    }

    /// <summary>
    /// Gets or sets RotateFinsished eventhandler.
    /// </summary>
    public event EventHandler<RotatedEventArgs> RotateFinsished
    {
        add
        {
            AddHandler(RotateFinsishedEvent, value);
        }
        remove
        {
            RemoveHandler(RotateFinsishedEvent, value);
        }
    }

    /// <summary>
    /// Gets or sets FillBrush.
    /// </summary>
    public IBrush FillBrush
    {
        get { return (IBrush)GetValue(FillBrushProperty); }
        set { SetValue(FillBrushProperty, value); }
    }

    /// <summary>
    /// Gets or sets StrokeBrush.
    /// </summary>
    public IBrush StrokeBrush
    {
        get { return (IBrush)GetValue(StrokeBrushProperty); }
        set { SetValue(StrokeBrushProperty, value); }
    }

    /// <summary>
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof(Thumb);

    /// <summary>
    /// remembers the current point if left button down
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        var currentPointerPoint = e.GetCurrentPoint(this);

        if (currentPointerPoint.Properties.IsLeftButtonPressed &&
            _designerItem != null && _canvas != null)
        {
            _currentPoint = e.GetPosition(_canvas);
        }
    }

    private void RotateThumb_DragCompleted(object sender, VectorEventArgs e)
    {
        var args = new RotatedEventArgs
        {
            Angle = _currentRotateAngle,
            Vector = e.Vector,
            Route = e.Route,
            Source = e.Source,
            RoutedEvent = RotateFinsishedEvent
        };

        RaiseEvent(args);
    }

    /// <summary>
    /// sets the angle to the rotate transform
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RotateThumb_DragDelta(object sender, VectorEventArgs e)
    {
        if (_designerItem != null && _canvas != null)
        {
            Vector deltaVector = _currentPoint - _centerPoint;
            double angle = VectorExtension.AngleBetween(_startVector, deltaVector);

            RotateTransform rotateTransform = _designerItem.RenderTransform as RotateTransform;

            _currentRotateAngle = rotateTransform.Angle = _initialAngle + Math.Round(angle, 0);
            _designerItem.InvalidateMeasure();
        }
    }

    /// <summary>
    /// sets the inital angle if datacontext is a ContentControl and if the Canvas was found.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RotateThumb_DragStarted(object sender, VectorEventArgs e)
    {
        _designerItem = DataContext as Control;

        if (_designerItem != null)
        {
            _canvas = TreeExtensions.TryFindParent<Canvas>(_designerItem);
            if (_canvas != null)
            {
                _centerPoint = (Point)_designerItem.TranslatePoint(
                    new Point(_designerItem.Width * _designerItem.RenderTransformOrigin.Point.X,
                              _designerItem.Height * _designerItem.RenderTransformOrigin.Point.Y),
                              _canvas);

                Point startPoint = VisualExtensions.
                PointToClient(_canvas, new PixelPoint((int)e.Vector.X, (int)e.Vector.Y));

                _startVector = startPoint - _centerPoint;

                _rotateTransform = _designerItem.RenderTransform as RotateTransform;
                if (_rotateTransform == null)
                {
                    _designerItem.RenderTransform = new RotateTransform(0);
                    _initialAngle = 0;
                }
                else
                {
                    _initialAngle = _rotateTransform.Angle;
                }
            }
        }
    }
}
