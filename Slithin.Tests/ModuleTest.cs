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

            var instance = ActionModule.Compile(m, imports);

            // var mem = instance.memory;
            var id = instance._start();

            ActionModule.RunExports();

            var jk = Mod.k;
        }

        private static class Mod
        {
            [WasmExportValue(50)]
            public static Point hello = new Point(12, 42);

            [WasmImportValue(125)]
            public static byte k;

            [WasmExportValue(125)]
            public static string world = "Hello World";
        }
    }
}