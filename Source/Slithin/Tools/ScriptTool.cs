using System;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.WasmInterface;
using Slithin.ModuleSystem;
using Slithin.ModuleSystem.StdLib;
using WebAssembly;

namespace Slithin.Tools;

public class ScriptTool : ITool
{
    private readonly ScriptInfo _info;
    private readonly Module _module;
    private readonly CustomSection uiSection;

    private dynamic instance;

    public ScriptTool(ScriptInfo info, Module module)
    {
        _info = info;
        _module = module;

        uiSection = _module.CustomSections.FirstOrDefault(_ => _.Name == ".ui");
    }

    public IImage Image
    {
        get
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var imageSection = _module.CustomSections.FirstOrDefault(_ => _.Name == ".image");

            Stream imageStream;
            if (imageSection != null)
            {
                imageStream = new MemoryStream(imageSection.Content.ToArray());
            }
            else
            {
                imageStream = assets.Open(new Uri("avares://Slithin/Resources/cubes.png"));
            }

            return new Bitmap(imageStream);
        }
    }

    public Models.ScriptInfo Info =>
        new(_info.Id, _info.Name, _info.Category, _info.Description, false, _info.IsListed);

    public bool IsConfigurable => uiSection != null;

    public Control GetModal()
    {
        if (uiSection == null)
        {
            return null;
        }

        return AvaloniaRuntimeXamlLoader.Parse<Control>(
            Encoding.ASCII.GetString(uiSection.Content.ToArray())
        );
    }

    public void Invoke(object data)
    {
        // var mem = instance.memory;
        instance._start();

        ActionModule.RunExports(instance);
    }

    public void Init()
    {
        var automation = ServiceLocator.Container.Resolve<Automation>();
        var imports = automation.Imports;

        ModuleImporter.Import(typeof(ConversionsImplementation), imports);
        ModuleImporter.Import(typeof(StringImplementation), imports);
        ModuleImporter.Import(typeof(NotificationImplementation), imports);

        ModuleImporter.Import(typeof(ModuleSystem.StdLib.Core), imports);

        instance = ActionModule.Compile(_module, imports);
    }
}
