using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Slithin.Core;
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

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.GetCommandLineArgs().Contains("--install"))
        {
            var copyCmd = UpdateScriptGenerator.GetWindowsScript(Environment.CurrentDirectory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Slithin"));

            Process.Start(new ProcessStartInfo("cmd", $"/c " + copyCmd) { CreateNoWindow = true });
            Environment.Exit(0);
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new ConnectWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}