using WebAssembly;

namespace Slithin.ModuleSystem
{
    public class ActionModule
    {
        public static Module LoadModule(string path)
        {
            var m = Module.ReadFromBinary(path);
            var wasi = new WASInterface.WASI(m);

            var imports = wasi.CreateImports();

            return m;
        }
    }
}