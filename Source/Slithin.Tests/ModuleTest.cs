using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using Slithin.ActionCompiler;
using Slithin.ModuleSystem;
using WebAssembly;
using WebAssembly.Runtime;

namespace Slithin.Tests;

[TestFixture]
public static class ModuleTest
{
    [Test]
    public static void Compile()
    {
        var m = ModuleCompiler.Compile("testScript", "testScript.info");

        m.WriteToBinary(File.OpenWrite("testScript.wasm"));
    }

    [Test]
    public static void Invoke()
    {
        var m = Module.ReadFromBinary("testScript.wasm");
        var r = m.Compile<dynamic>();
        var rr = r(new ImportDictionary
        {
            ["env"] = new Dictionary<string, RuntimeImport>
            {
                ["memory"] = new MemoryImport(() => new UnmanagedMemory(1, 2))
            }
        });
        var rrr = rr.Exports._start();
    }

    [Test]
    public static void Import()
    {
        var m = ActionModule.LoadModule("main.wasm", out var imports);

        ModuleImporter.Import(typeof(Mod), imports);

        var instance = ActionModule.Compile(m, imports);

        // var mem = instance.memory;
        var id = instance._start();

        ActionModule.RunExports();

        var jk = Mod.kc;
    }

    private static class Mod
    {
        [WasmExportValue(50)] public static Point hello = new(12, 42);

        [WasmImportValue(125)] public static string k;

        [WasmImportValue(125)] public static char kc;

        [WasmExportValue(125)] public static string world = "Hello World";
    }
}