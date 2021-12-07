using Furesoft.Core.CodeDom.CodeDOM.Base;

namespace Slithin.ActionCompiler.Compiling;

public interface IPass
{
    CodeObject Process(CodeObject obj, PassManager passManager);

    public Block Process(Block block, PassManager passManager)
    {
        var result = new Block();

        foreach (var item in block)
        {
            result.Add(Process(item, passManager));
        }

        return result;
    }
}