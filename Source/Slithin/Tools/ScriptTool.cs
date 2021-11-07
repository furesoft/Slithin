using System;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.ModuleSystem;
using WebAssembly;

namespace Slithin.Tools
{
    public class ScriptTool : ITool
    {
        private readonly Slithin.ModuleSystem.ScriptInfo _info;
        private readonly Module _module;
        private readonly CustomSection uiSection;

        public ScriptTool(Slithin.ModuleSystem.ScriptInfo info, Module module)
        {
            _info = info;
            _module = module;

            uiSection = _module.CustomSections.FirstOrDefault(_ => _.Name == ".ui");
        }

        //ToDo: enable custom image
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
                    imageStream = assets.Open(new Uri($"avares://Slithin/Resources/cubes.png"));
                }

                return new Bitmap(imageStream);
            }
        }

        public Models.ScriptInfo Info => new(_info.ID, _info.Name, _info.Category, _info.Description, false);

        public bool IsConfigurable => uiSection != null;

        public Control GetModal()
        {
            if (uiSection == null)
                return null;

            return Avalonia.Markup.Xaml.AvaloniaRuntimeXamlLoader.Parse<Control>(
                Encoding.ASCII.GetString(uiSection.Content.ToArray())
                );
        }

        public void Invoke(object data)
        {
            var automation = ServiceLocator.Container.Resolve<Automation>();
            var imports = automation.Imports;

            var instance = ActionModule.Compile(_module, imports);

            // var mem = instance.memory;
            instance._start();

            ActionModule.RunExports();
        }
    }
}
