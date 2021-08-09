using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.UI.Views;
using Slithin.Core.Scripting;

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
        }
    }
}
