using System;

namespace Slithin.ModuleSystem;

[AttributeUsage(AttributeTargets.Field)]
public class WasmImportGlobalAttribute : Attribute
{
    public WasmImportGlobalAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}