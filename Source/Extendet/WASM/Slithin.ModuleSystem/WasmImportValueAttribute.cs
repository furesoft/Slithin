﻿using System;

namespace Slithin.ModuleSystem;

[AttributeUsage(AttributeTargets.Field)]
public class WasmImportValueAttribute : Attribute
{
    public WasmImportValueAttribute(int offset)
    {
        Offset = offset;
    }

    public int Length { get; init; }
    public int Offset { get; }
}