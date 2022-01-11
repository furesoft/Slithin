using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Statements.Variables.Base;

namespace Slithin.ActionCompiler.Compiling.Passes;

public class BooleanFoldingPass : IPass
{
    public CodeObject Process(CodeObject obj, PassManager passManager)
    {
        if (obj is VariableDecl vardec && vardec.Initialization is Literal lit)
        {
            if (lit.Text == "false")
            {
                vardec.Initialization = new Literal(0);
            }
            else if (lit.Text == "true")
            {
                vardec.Initialization = new Literal(1);
            }
        }

        return obj;
    }
}