using Furesoft.Core.CodeDom.CodeDOM;
using Furesoft.Core.CodeDom.CodeDOM.Base;
using System.Collections.Generic;

namespace Slithin.ActionCompiler.Compiling;

public class PassManager
{
    private readonly List<IPass> Passes = new();

    public void AddPass<T>()
        where T : IPass, new()
    {
        Passes.Add(new T());
    }

    public CodeUnit Process(CodeUnit obj)
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

    public Block ProcessBlock(Block block)
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

    private void Process(Block result, CodeObject t)
    {
        foreach (var pass in Passes)
        {
            //ToDo: need to be fixed: only should return 1 object not 1 object each pass
            var processedObj = pass.Process(t, this);
            if (processedObj is Block blk)
            {
                result.AddRange(blk);
            }
            else
            {
                result.Add(processedObj);
            }
        }
    }
}