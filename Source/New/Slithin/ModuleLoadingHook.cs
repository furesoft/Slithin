using AuroraModularis.Core.Hooks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;

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
        var resDictionary = GetFromResource<ResourceDictionary>(type, "Icons");

        if (resDictionary is null) return;

        Application.Current!.Resources.MergedDictionaries.Add(resDictionary);
    }

    private static void RegisterDataTemplates(Type type)
    {
        var dataTemplates = GetFromResource<DataTemplates>(type, "DataTemplates");
        if (dataTemplates is null) return;

        Application.Current!.DataTemplates.AddRange(dataTemplates);
    }

    private static T? GetFromResource<T>(Type type, string name)
    {
        var stream = type.Assembly.GetManifestResourceStream($"{type.Assembly.GetName().Name}.Resources.{name}.xml");
        if (stream is null) return default;

        return (T?)AvaloniaRuntimeXamlLoader.Load(stream, type.Assembly);
    }
}
