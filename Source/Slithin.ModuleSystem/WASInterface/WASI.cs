using System;
using System.Collections.Generic;
using WebAssembly;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem.WASInterface;

public class WASI
{
    public WASI(Module module)
    {
        Module = module;

        Memory = new UnmanagedMemory(1, 255);

        Sg_wasm.Mem = Memory.Start;
        Sg_wasm.MemSize = (int) Memory.Size;
    }

    public UnmanagedMemory Memory { get; init; }
    public Module Module { get; }

    public ImportDictionary CreateImports()
    {
        var importss = new ImportDictionary();
        importss.Add("wasi_unstable", new Dictionary<string, RuntimeImport>
        {
            ["fd_write"] = new FunctionImport(new Func<int, int, int, int, int>(WasiUnstable.fd_write)),
            ["fd_read"] = new FunctionImport(new Func<int, int, int, int, int>(WasiUnstable.fd_read)),
            ["path_open"] =
                new FunctionImport(
                    new Func<int, int, int, int, int, long, long, int, int, int>(WasiUnstable.path_open)),
            ["proc_exit"] = new FunctionImport(new Action<int>(WasiUnstable.proc_exit))
        });
        importss.Add("env", new Dictionary<string, RuntimeImport>
        {
            ["memory"] = new MemoryImport(() => Memory)
        });

        return importss;
    }
}