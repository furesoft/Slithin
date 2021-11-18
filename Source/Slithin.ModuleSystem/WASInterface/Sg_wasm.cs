using System;

namespace Slithin.ModuleSystem.WASInterface;

public static class Sg_wasm
{
    public static IntPtr Mem;
    public static int MemSize;

    public static Span<byte> GetSpan(int addr, int len)
    {
        unsafe
        {
            return new Span<byte>((Mem + addr).ToPointer(), len);
        }
    }
}