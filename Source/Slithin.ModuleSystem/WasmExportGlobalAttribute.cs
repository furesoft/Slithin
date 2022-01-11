using System;

namespace Slithin.ModuleSystem;

[AttributeUsage(AttributeTargets.Field)]
public class WasmExportGlobalAttribute : Attribute
{
    public WasmExportGlobalAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}