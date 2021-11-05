﻿using NUnit.Framework;
using Slithin.ActionCompiler;
using Slithin.ModuleSystem;
using System.Drawing;

namespace Slithin.Tests
{
    [NUnit.Framework.TestFixture]
    public static class ModuleTest
    {
        [Test]
        public static void Compile()
        {
            ModuleCompiler.Compile("testScript", "testScript.info", "testScript.ui");
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
            [WasmExportValue(50)]
            public static Point hello = new Point(12, 42);

            [WasmImportValue(125)]
            public static string k;

            [WasmImportValue(125)]
            public static char kc;

            [WasmExportValue(125)]
            public static string world = "Hello World";
        }
    }
}