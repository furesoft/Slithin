using System;

namespace Slithin.ModuleSystem.WASInterface
{
    public static class Sg_wasm
    {
        public static IntPtr __mem;
        public static int __mem_size;

        public static Span<byte> GetSpan(int addr, int len)
        {
            unsafe
            {
                return new Span<byte>((Sg_wasm.__mem + (int)addr).ToPointer(), len);
            }
        }
    }
}