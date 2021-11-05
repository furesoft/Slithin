using NUnit.Framework;
using Slithin.ModuleSystem;

namespace Slithin.Tests
{
    [NUnit.Framework.TestFixture]
    public static class ModuleTest
    {
        [Test]
        public static void Import()
        {
            var m = ActionModule.LoadModule("main.wasm", out var imports);

            var compiled = m.Compile<dynamic>();
            var instance = compiled(imports);

            var mem = instance.Exports.memory;
            var id = instance.Exports._start();
        }
    }
}