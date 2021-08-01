using System;
using System.IO;
using Actress;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Messages;
using Slithin.UI.Views;

namespace Slithin
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ServiceLocator.Init();

            Automation.Init();

            Automation.Evaluate("testScript");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new ConnectWindow();
            }

            var root = new DirectoryInfo(ServiceLocator.ConfigBaseDir);
            var tmpls = new DirectoryInfo(ServiceLocator.TemplatesDir);
            var notes = new DirectoryInfo(ServiceLocator.NotebooksDir);
            var scripts = new DirectoryInfo(ServiceLocator.ScriptsDir);
            var screens = new DirectoryInfo(ServiceLocator.CustomScreensDir);

            if (!root.Exists || !tmpls.Exists || !notes.Exists || !scripts.Exists || !screens.Exists)
            {
                try
                {
                    root.Create();
                    tmpls.Create();
                    notes.Create();
                    scripts.Create();
                    screens.Create();
                }
                catch { }

                File.WriteAllText(Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json"), "{\"templates\": []}");
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
