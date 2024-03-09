﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Controls;

public class ImageButton : Button
{
    public static readonly StyledProperty<Geometry> ImageProperty =
        AvaloniaProperty.Register<ImageButton, Geometry>("Kind");

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ImageButton, string>("Text");

    public Geometry Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
