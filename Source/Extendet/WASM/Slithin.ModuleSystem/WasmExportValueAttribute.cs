using System;

namespace Slithin.ModuleSystem;

[AttributeUsage(AttributeTargets.Field)]
public class WasmExportValueAttribute : Attribute
{
    public WasmExportValueAttribute(int offset)
    {
        Offset = offset;
    }

    public int Offset { get; set; }
}