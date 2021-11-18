using Furesoft.Core.CodeDom.CodeDOM.Base;

namespace Slithin.ActionCompiler.Compiling;

public interface IPass
{
    CodeObject Process(CodeObject obj);
}