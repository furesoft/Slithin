using AuroraModularis.Hooks.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Portable.Xaml;

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
        var resDictionary = GetFromResource<ResourceDictionary>(type);

        if (resDictionary is null) return;
        
        Application.Current!.Resources.MergedDictionaries.Add(resDictionary);
    }
    
    private static void RegisterDataTemplates(Type type)
    {
        var dataTemplates = GetFromResource<DataTemplates>(type);
        if (dataTemplates is null) return;
        
        Application.Current!.DataTemplates.AddRange(dataTemplates);
    }

    private static T? GetFromResource<T>(Type type)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        var uri = new Uri($"avares://{type.Namespace}/Resources/DataTemplates.xml");
        if (assets!.Exists(uri))
        {
            return (T) XamlServices.Load(assets.Open(uri));
        }

        return default;
    }
}
