using System;

namespace Slithin.ModuleSystem
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class WasmExportAttribute : Attribute
    {
        public WasmExportAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}