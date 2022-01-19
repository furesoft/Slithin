﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages;

[PreserveIndex(5)]
public partial class SettingsPage : UserControl, IPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    public string Title => "Settings";

    public Control GetContextualMenu() => null;

    bool IPage.IsEnabled() => true;

    public bool UseContextualMenu() => false;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<SettingsPageViewModel>();
    }
}