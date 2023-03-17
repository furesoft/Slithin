using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Threading;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Bot.ViewModels;
using Slithin.Modules.UI.Models;
using Syn.Bot.Common;
using Syn.Bot.Oscova;

namespace Slithin.Modules.Bot;

[Priority(ModulePriority.Low)]
public class Module : AuroraModularis.Module
{
    private OscovaBot _bot;
    public override Task OnStart(ServiceContainer container)
    {
       _bot= ServiceContainer.Current.Resolve<OscovaBot>();

       InitBotSettings();
       
       container.Resolve<IEventService>().Subscribe<object>("ApplicationLoaded", async _ =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                AddChatButton(container);
            });
        });

        return Task.CompletedTask;
    }

    private void InitBotSettings()
    {
        foreach (var setting in (Dictionary<string, string>) Settings)
        {
            _bot.Settings.Add(setting.Key, setting.Value);
        }
    }

    public override void OnInit()
    {
        UseSettings = true;

        Settings = new Dictionary<string, string>();
    }

    public override void OnExit()
    {
        var botSettings = _bot.Settings;
        Settings = botSettings.Where(_=> _ is Variable).ToDictionary(_ => _.Name, _ => _.Value);

        base.OnExit();
    }

    public override void RegisterServices(ServiceContainer serviceContainer)
    {
        serviceContainer.Register(new OscovaBot());
    }

    private static void AddChatButton(ServiceContainer container)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var dsHost = container.Resolve<IDialogService>().GetContainerControl();
            var grid = ((Control)dsHost.Content).FindControl<Grid>("contentGrid");

            var botChatButton = new BotChatButton
            {
                ZIndex = 1000, DataContext = container.Resolve<ChatFlyoutViewModel>()
            };

            Grid.SetColumnSpan(botChatButton, 5);
            Grid.SetRowSpan(botChatButton, 5);

            grid.Children.Add(botChatButton);
        });
    }
}
