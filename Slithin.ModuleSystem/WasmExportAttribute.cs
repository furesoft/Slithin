using System;

namespace Slithin.ModuleSystem
{
    public class WasmExportAttribute : Attribute
    {
        public WasmExportAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}