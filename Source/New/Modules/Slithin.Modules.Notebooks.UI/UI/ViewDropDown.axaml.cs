﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Notebooks.UI.UI;

public partial class ViewDropDown : UserControl
{
    public ViewDropDown()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

