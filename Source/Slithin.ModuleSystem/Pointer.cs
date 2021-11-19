﻿using System;
using System.Runtime.InteropServices;
using Slithin.ModuleSystem.WASInterface;

namespace Slithin.ModuleSystem;

public struct Pointer
{
    public Pointer(int offset) : this()
    {
        Base = offset;
    }

    private int Base { get; init; }

    public static implicit operator IntPtr(Pointer grain)
    {
        return new IntPtr(grain.Base);
    }

    public static implicit operator Pointer(IntPtr grain)
    {
        return new Pointer {Base = grain.ToInt32()};
    }

    public int this[int offset]
    {
        get => ReadInt32(offset);
        set => Write(value, offset);
    }

    public static implicit operator Pointer(int grain)
    {
        return new Pointer {Base = grain};
    }

    public int ReadInt32(int offset = 0)
    {
        return Marshal.ReadInt32(Sg_wasm.Mem + Base + offset);
    }

    public void Write(int value, int offset = 0)
    {
        Marshal.WriteInt32(Sg_wasm.Mem + Base + offset, value);
    }

    public void Write(string value, int offset = 0)
    {
        var utf8 = Util.ToUtf8(value);

        Marshal.Copy(utf8, 0, Sg_wasm.Mem + Base + offset, utf8.Length);
    }

    public void Write(byte value)
    {
        Marshal.WriteByte(Sg_wasm.Mem + Base, value);
    }

    public static Pointer operator ++(Pointer ptr)
    {
        return new Pointer(ptr.Base + 1);
    }

    public static Pointer operator --(Pointer ptr)
    {
        return new Pointer(ptr.Base - 1);
    }
}