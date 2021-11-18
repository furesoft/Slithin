using System.Collections.Generic;
using Furesoft.Core.CodeDom.CodeDOM.Base;

namespace Slithin.ActionCompiler.Compiling;

public static class PassManager
{
    private static readonly List<IPass> _passes = new();

    public static void AddPass<T>()
        where T : IPass, new()
    {
        _passes.Add(new T());
    }

    public static Block Process(Block obj)
    {
        var result = new Block();
        foreach (var t in obj)
        {
            if (t is Block blk) result.Add(Process(blk));

            foreach (var pass in _passes)
                result.Add(pass.Process(t));
        }


        return result;
    }
}