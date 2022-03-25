using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiteDB;
using Slithin.Core;
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

        var cvm = ServiceLocator.Container.Resolve<ConnectionWindowViewModel>();
        cvm.Load();

        DataContext = cvm;

        Closed += (s, e) =>
        {
            var db = ServiceLocator.Container.Resolve<LiteDatabase>();

            db.Dispose();
        };
    }
}
