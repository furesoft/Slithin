﻿using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.UI.Views;

namespace Slithin;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ServiceLocator.Init();

        var settings = ServiceLocator.Container.Resolve<ISettingsService>().GetSettings();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new ConnectWindow();
        }

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
          {
              ServiceLocator.Container.Resolve<IErrorTrackingService>().TrackException(e.ExceptionObject as Exception);
              NotificationService.ShowError(e.ExceptionObject.ToString());
          };

        base.OnFrameworkInitializationCompleted();
    }
}
