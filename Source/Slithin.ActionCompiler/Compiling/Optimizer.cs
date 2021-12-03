using System.Collections.Generic;
using Furesoft.Core.CodeDom.CodeDOM;
using Furesoft.Core.CodeDom.CodeDOM.Base;

namespace Slithin.ActionCompiler.Compiling;

public static class Optimizer
{
    private static readonly List<IPass> Passes = new();

    public static void AddPass<T>()
        where T : IPass, new()
    {
        Passes.Add(new T());
    }

    public static CodeUnit Process(CodeUnit obj)
    {
        var result = new Block();

        foreach (var t in obj.Body)
        {
            if (t is Block blk) result.Add(ProcessBlock(blk));

            Process(result, t);

        }

        obj.Body = result;

        return obj;
    }

    private static void Process(Block result, CodeObject t)
    {
        foreach (var pass in Passes)
            result.Add(pass.Process(t));
    }

    private static Block ProcessBlock(Block block)
    {
        var result = new Block();

        foreach (var t in block)
        {
            if (t is Block blk)
            {
                result.Add(ProcessBlock(blk));
                continue;
            }

            Process(result, t);
        }

        return result;
    }
}