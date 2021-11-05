using Slithin.ModuleSystem.WASInterface;
using System;
using System.Runtime.InteropServices;

namespace Slithin.ModuleSystem
{
    public struct Pointer
    {
        public Pointer(int offset) : this()
        {
            Base = offset;
        }

        public int Base { get; set; }

        public static implicit operator IntPtr(Pointer grain)
        {
            return new(grain.Base);
        }

        public static implicit operator Pointer(IntPtr grain)
        {
            return new() { Base = grain.ToInt32() };
        }

        public static implicit operator Pointer(int grain)
        {
            return new() { Base = grain };
        }

        public int ReadInt32()
        {
            return Marshal.ReadInt32(Sg_wasm.__mem + Base);
        }

        public void Write(int value)
        {
            Marshal.WriteInt32(Sg_wasm.__mem + Base, value);
        }

        public void Write(byte value)
        {
            Marshal.WriteByte(Sg_wasm.__mem + Base, value);
        }
    }
}