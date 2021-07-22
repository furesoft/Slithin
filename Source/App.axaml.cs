using System;
using System.IO;
using Actress;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Slithin.Core;
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
#if DEBUG
                desktop.MainWindow = new ConnectWindow();
#else
                desktop.MainWindow = new MainWindow();
#endif
            }

            ServiceLocator.Mailbox = MailboxProcessor.Start<AsynchronousMessage>(
                async (_) =>
                {
                    while (true)
                    {
                        var msg = await _.Receive();

                        MessageRouter.Route(msg);
                    }
                }
                );

            ServiceLocator.InitMessageRouter();

            ServiceLocator.SyncService.LoadFromLocal();

            base.OnFrameworkInitializationCompleted();
        }
    }
}
