using System;
using System.Collections.Generic;
using WebAssembly;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem.WASInterface;

public class Wasi
{
    public Wasi(Module module)
    {
        Module = module;

        Memory = new UnmanagedMemory(1, 255);

        WasmMemory.Mem = Memory.Start;
    }

    public UnmanagedMemory Memory { get; }
    public Module Module { get; }

    public ImportDictionary CreateImports()
    {
        var importss = new ImportDictionary
        {
            {
                "wasi_unstable", new Dictionary<string, RuntimeImport>
                {
                    ["fd_write"] = new FunctionImport(new Func<int, int, int, int, int>(WasiUnstable.fd_write)),
                    ["fd_read"] = new FunctionImport(new Func<int, int, int, int, int>(WasiUnstable.fd_read)),
                    ["path_open"] =
                        new FunctionImport(
                            new Func<int, int, int, int, int, long, long, int, int, int>(WasiUnstable.path_open)),
                    ["proc_exit"] = new FunctionImport(new Action<int>(WasiUnstable.proc_exit))
                }
            },
            {
                "env", new Dictionary<string, RuntimeImport>
                {
                    ["memory"] = new MemoryImport(() => Memory)
                }
            }
        };

        return importss;
    }
}