using Avalonia;
using Avalonia.Interactivity;

namespace Slithin.Controls.Ports.ResizeRotateControl;

/// <summary>
/// event if the DragFinished is execute on the <see cref="RotateThumb"/>
/// </summary>
public class RotatedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Angle
    /// </summary>
    /// <value></value>
    public double Angle { get; set; }

    /// <summary>
    /// moved position
    /// </summary>
    /// <value></value>
    public Vector Vector { get; set; }
}
