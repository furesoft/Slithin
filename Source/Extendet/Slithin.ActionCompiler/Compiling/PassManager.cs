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

        obj.Body = ProcessBlock(obj.Body);

        return obj;
    }

    public Block ProcessBlock(Block block)
    {
        foreach (var pass in Passes)
        {
            //ToDo: need to be fixed: only should return 1 object not 1 object each pass
            block = pass.Process(block, this);
        }

        return block;
    }
}