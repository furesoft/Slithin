using AuroraModularis.Core;
using AuroraModularis.Hooks.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;

namespace Slithin;

public class ModuleLoadingHook : IModuleLoadingHook
{
    public bool ShouldLoadModule(AuroraModularis.Module module)
    {
        return true;
    }

    public void BeforeLoadModule(Type moduleType)
    {
        RegisterIconsFrom(moduleType);
        
        RegisterDataTemplates(moduleType);
    }

    public void AfterLoadModule(AuroraModularis.Module module)
    {
        
    }

    private static void RegisterIconsFrom(Type type)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        var uri = new Uri("avares://" + type.Namespace + "/Resources/Icons.xml");
        if (assets.Exists(uri))
        {
            var resDictionary = (ResourceDictionary)AvaloniaRuntimeXamlLoader.Load(assets.Open(uri), type.Assembly);

            App.Current.Resources.MergedDictionaries.Add(resDictionary);
        }
    }
    
    private static void RegisterDataTemplates(Type type)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        var uri = new Uri("avares://" + type.Namespace + "/Resources/DataTemplates.xml");
        if (assets.Exists(uri))
        {
            var dataTemplates = (DataTemplates)AvaloniaRuntimeXamlLoader.Load(assets.Open(uri), type.Assembly);

            Application.Current.DataTemplates.AddRange(dataTemplates);
        }
    }
}
