using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Types;
using Slithin.ActionCompiler.Parsing.AST;
using System;

namespace Slithin.ActionCompiler.Compiling.Passes;

public class TypeResolvePass : IPass
{
    public CodeObject Process(CodeObject obj, PassManager passManager)
    {
        if (obj is VarDecl varDecl && varDecl.Type == null)
        {
            ResolveVarType(varDecl);
        }


        return obj;
    }

    private void ResolveVarType(VarDecl varDecl)
    {
        var value = varDecl.Initialization;
        
        //literal, unresolvedRef for vardecl
        if (value is Literal lit)
        {
            varDecl.Type = new TypeRef(lit.Value);

            if (lit.Value is bool)
            {
                varDecl.Type = new TypeRef(lit);
            }
            else if (lit.Value is int)
            {

            }
            else if (lit.Value is long)
            {

            }
            else if (lit.Value is float)
            {

            }
            else if (lit.Value is double)
            {

            }
        }
    }
}
