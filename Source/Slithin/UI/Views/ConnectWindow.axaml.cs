using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiteDB;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.ViewModels;

namespace Slithin.UI.Views;

public partial class ConnectWindow : Window
{
    public ConnectWindow()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var li = ServiceLocator.Container.Resolve<ILoginService>().GetLoginCredentials();
        var cvm = ServiceLocator.Container.Resolve<ConnectionWindowViewModel>();

        for (var i = 0; i < li.Length; i++)
        {
            if (string.IsNullOrEmpty(li[i].Name))
            {
                li[i].Name = "Device " + (i + 1);
            }
        }

        cvm.SelectedLogin = li.FirstOrDefault() ?? new Models.LoginInfo();

        cvm.LoginCredentials = new(li);

        DataContext = cvm;

        Closed += (s, e) =>
        {
            var db = ServiceLocator.Container.Resolve<LiteDatabase>();

            db.Dispose();
        };
    }
}