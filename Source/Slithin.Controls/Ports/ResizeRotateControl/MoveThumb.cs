﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Slithin.Controls.Ports.Extensions;

namespace Slithin.Controls.Ports.ResizeRotateControl;

//ported from https://www.codeproject.com/Articles/22952/WPF-Diagram-Designer-Part-1

/// <summary>
/// move thumb control
/// </summary>
public class MoveThumb : Thumb
{
    /// <summary>
    /// Defines the <see cref="AllowDragOutOfView"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> AllowDragOutOfViewProperty =
        AvaloniaProperty.Register<MoveThumb, bool>(nameof(AllowDragOutOfView));

    /// <summary>
    /// Defines the <see cref="BouncedControl"/> property.
    /// </summary>
    public static readonly StyledProperty<Control> BouncedControlProperty =
        AvaloniaProperty.Register<MoveThumb, Control>(nameof(BouncedControl));

    /// <summary>
    /// Defines the <see cref="MovedFinished"/> routed event.
    /// </summary>
    public static readonly RoutedEvent<VectorEventArgs> MovedFinishedEvent =
                RoutedEvent.Register<MoveThumb, VectorEventArgs>(nameof(MovedFinishedEvent), RoutingStrategies.Bubble);

    private Control _designerItem;
    private bool _isDragStarted;
    private RotateTransform _rotateTransform;

    /// <summary>
    /// registers DragStarted, DragDelta
    /// </summary>
    public MoveThumb()
    {
        DragStarted += MoveThumb_DragStarted;
        DragDelta += MoveThumb_DragDelta;
        DragCompleted += MoveThumb_DragCompleted;
    }

    /// <summary>
    /// Gets or sets MovedFinished eventhandler.
    /// </summary>
    public event EventHandler<VectorEventArgs> MovedFinished
    {
        add
        {
            AddHandler(MovedFinishedEvent, value);
        }
        remove
        {
            RemoveHandler(MovedFinishedEvent, value);
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
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof(Thumb);

    private void MoveThumb_DragCompleted(object sender, VectorEventArgs e)
    {
        if (_isDragStarted == false)
        {
            return;
        }
        var arg = new VectorEventArgs
        {
            Handled = e.Handled,
            Route = e.Route,
            RoutedEvent = MovedFinishedEvent,
            Source = e.Source,
            Vector = e.Vector
        };
        RaiseEvent(arg);
        _isDragStarted = false;
    }

    /// <summary>
    /// sets the canvas through the dragDelta
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveThumb_DragDelta(object sender, VectorEventArgs e)
    {
        if (_designerItem != null && _isDragStarted)
        {
            Point dragDelta = new Point(e.Vector.X, e.Vector.Y);

            if (_rotateTransform != null)
            {
                dragDelta = _rotateTransform.Value.Transform(dragDelta);
            }

            var left = Canvas.GetLeft(_designerItem) + dragDelta.X;
            var top = Canvas.GetTop(_designerItem) + dragDelta.Y;
            var right = Canvas.GetLeft(_designerItem) + _designerItem.Width;
            var bottom = Canvas.GetTop(_designerItem) + _designerItem.Height;

            if (AllowDragOutOfView == false)
            {
                var bouncedControl = BouncedControl != null ? BouncedControl : _designerItem.Parent;

                var controlBounds = bouncedControl.Bounds;

                var rect = new Rect(new Point(left, top), _designerItem.DesiredSize);

                if (controlBounds.Contains(rect) == false)
                {
                    return;
                }
            }

            Canvas.SetLeft(_designerItem, left);
            Canvas.SetTop(_designerItem, top);

            Canvas.SetRight(_designerItem, right);
            Canvas.SetBottom(_designerItem, bottom);
        }
    }

    /// <summary>
    /// sets the rotatateTransform from the contentcontrol
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveThumb_DragStarted(object sender, VectorEventArgs e)
    {
        _designerItem = DataContext as Control;

        if (_designerItem != null)
        {
            _rotateTransform = _designerItem.RenderTransform as RotateTransform;

            _isDragStarted = true;
        }
    }
}
