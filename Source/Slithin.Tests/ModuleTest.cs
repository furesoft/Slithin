using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using Slithin.ActionCompiler;
using Slithin.ModuleSystem;
using Slithin.ModuleSystem.StdLib;
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

        using var fileStream = File.Create("../testScript.wasm");
        m.WriteToBinary(fileStream);
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

        rr.Exports._start();
    }

    [Test]
    public static void Import()
    {
        var m = ActionModule.LoadModule("../testScript.wasm", out var imports);

        ModuleImporter.Import(typeof(Mod), imports);
        ModuleImporter.Import(typeof(ConversionsImplementation), imports);
        ModuleImporter.Import(typeof(StringImplementation), imports);
        ModuleImporter.Import(typeof(Allocator), imports);
        ModuleImporter.Import(typeof(ModuleSystem.StdLib.Core), imports);


        var instance = ActionModule.Compile(m, imports);


        // var mem = instance.memory;
        instance._start();

        ActionModule.RunExports(instance);

        var jk = Mod.heapBase;
    }

    private static class Mod
    {
        [WasmExportValue(50)] public static Point hello = new(12, 42);

        [WasmImportValue(125)] public static string k;

        [WasmImportValue(125)] public static char kc;

        [WasmExportValue(125)] public static readonly string world = "Hello World";

        [WasmImportGlobal("_heap_base")] public static int heapBase;
    }
}