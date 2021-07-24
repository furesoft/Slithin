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

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
#if !DEBUG
                desktop.MainWindow = new ConnectWindow();
#else
                desktop.MainWindow = new MainWindow();
#endif
            }

            var root = new DirectoryInfo(ServiceLocator.ConfigBaseDir);
            var tmpls = new DirectoryInfo(ServiceLocator.TemplatesDir);
            var notes = new DirectoryInfo(ServiceLocator.NotebooksDir);

            if (!root.Exists || !tmpls.Exists || !notes.Exists)
            {
                try
                {
                    root.Create();
                    tmpls.Create();
                    notes.Create();
                }
                catch { }

                File.WriteAllText(Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json"), "{\"templates\": []}");
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
