using System.Collections.Generic;
using Furesoft.Core.CodeDom.CodeDOM.Base;

namespace Slithin.ActionCompiler.Compiling;

public static class Optimiser
{
    private static readonly List<IPass> Passes = new();

    public static void AddPass<T>()
        where T : IPass, new()
    {
        Passes.Add(new T());
    }

    public static Block Process(Block obj)
    {
        var result = new Block();
        foreach (var t in obj)
        {
            if (t is Block blk) result.Add(Process(blk));

            foreach (var pass in Passes)
                result.Add(pass.Process(t));
        }


        return result;
    }
}