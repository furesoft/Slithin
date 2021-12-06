using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Types;
using Slithin.ActionCompiler.Parsing.AST;

namespace Slithin.ActionCompiler.Compiling.Passes;

public class TypeResolvePass : IPass
{
    public CodeObject Process(CodeObject obj, PassManager passManager)
    {
        if (obj is VarDecl varDecl && varDecl.Type == null) ResolveVarType(varDecl);

        return obj;
    }

    private void ResolveVarType(VarDecl varDecl)
    {
        var value = varDecl.Initialization;

        //literal, unresolvedRef for vardecl
        if (value is Literal lit)
        {
            varDecl.Type = new TypeRef(lit.Value);

            if (bool.TryParse(lit.Text, out var boolLit))
            {
                varDecl.Type = new TypeRef(boolLit);
            }
            else if (bool.TryParse(lit.Text, out var boolLit))
            {
                varDecl.Type = new TypeRef(boolLit);
            }
            else if (bool.TryParse(lit.Text, out var boolLit))
            {
                varDecl.Type = new TypeRef(boolLit);
            }
        }
    }
}