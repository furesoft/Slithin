﻿using System;

namespace Slithin.ModuleSystem.WASInterface;

public static class WasmMemory
{
    public static IntPtr Mem;

    public static Span<byte> GetSpan(int addr, int len)
    {
        unsafe
        {
            return new Span<byte>((Mem + addr).ToPointer(), len);
        }
    }
}