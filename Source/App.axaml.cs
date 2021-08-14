using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Remarkable.LinesAreBeatiful;
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
                desktop.MainWindow = new ConnectWindow();
            }

            base.OnFrameworkInitializationCompleted();

            var notebook = Notebook.Load("aa69f2cc-7af8-4f6e-9977-d9ffc902c1d2");
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test-export";
            notebook.Export(path, ExportType.SVG);
        }
    }
}
