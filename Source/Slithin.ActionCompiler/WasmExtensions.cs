using System.Collections.Generic;
using System.Linq;
using Slithin.ModuleSystem.WASInterface;
using WebAssembly;
using WebAssembly.Instructions;

namespace Slithin.Core;

public static class WasmExtensions
{
    public static void AddData(this Module m, int index, byte[] raw)
    {
        m.Data.Add(new Data
        {
            Index = 0,
            InitializerExpression = new List<Instruction> {new Int32Constant(index), new End()},
            RawData = raw.ToList()
        });
    }

    public static void AddData(this Module m, int index, string value)
    {
        var raw = Util.ToUtf8(value);
        //raw = raw.Append((byte) 0).ToArray();

        AddData(m, index, raw);
    }
}