using WebAssembly;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem
{
    public class ActionModule
    {
        public static Module LoadModule(string path, out ImportDictionary imports)
        {
            var m = Module.ReadFromBinary(path);
            var wasi = new WASInterface.WASI(m);

            imports = wasi.CreateImports();

            return m;
        }
    }
}