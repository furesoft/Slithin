using NUnit.Framework;
using Slithin.ModuleSystem;
using System.Drawing;

namespace Slithin.Tests
{
    [NUnit.Framework.TestFixture]
    public static class ModuleTest
    {
        [Test]
        public static void Import()
        {
            var m = ActionModule.LoadModule("main.wasm", out var imports);

            ModuleImporter.Import(typeof(Mod), imports);

            var compiled = m.Compile<dynamic>();
            var instance = compiled(imports);

            var mem = instance.Exports.memory;
            var id = instance.Exports._start();
        }

        private static class Mod
        {
            [WasmExportValue(50)]
            public static Point hello = new Point(12, 42);

            [WasmExportValue(125)]
            public static string world = "Hello World";
        }
    }
}