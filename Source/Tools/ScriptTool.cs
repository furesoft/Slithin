using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Models;
using Slithin.ModuleSystem;
using WebAssembly;

namespace Slithin.Tools
{
    public class ScriptTool : ITool
    {
        private readonly ActionCompiler.ScriptInfo _info;
        private readonly Module _module;

        public ScriptTool(ActionCompiler.ScriptInfo info, Module module)
        {
            _info = info;
            _module = module;
        }

        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/cubes.png")));
            }
        }

        public ScriptInfo Info => new(_info.ID, _info.Name, _info.Category, _info.Description, false);

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            throw new NotImplementedException();
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
