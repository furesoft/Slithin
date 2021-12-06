using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Types;
using Slithin.ActionCompiler.Parsing.AST;
using System.Globalization;

namespace Slithin.ActionCompiler.Compiling.Passes;

public class TypeResolvePass : IPass
{
    public CodeObject Process(CodeObject obj, PassManager passManager)
    {
        if (obj is VarDecl varDecl && varDecl.Type == null)
            ResolveVarType(varDecl);

        return obj;
    }

    private void ResolveVarType(VarDecl varDecl)
    {
        var value = varDecl.Initialization;

        //literal, unresolvedRef for vardecl
        if (value is Literal lit)
        {
            if (bool.TryParse(lit.Text, out var boolLit))
                varDecl.Type = new TypeRef(boolLit.GetType());
            else if (int.TryParse(lit.Text, out var intLit))
                varDecl.Type = new TypeRef(intLit.GetType());
            else if (long.TryParse(lit.Text, out var longLit))
                varDecl.Type = new TypeRef(longLit.GetType());
            else if (float.TryParse(lit.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                         out var floatLit))
                varDecl.Type = new TypeRef(floatLit.GetType());
            else if (double.TryParse(lit.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                         out var doubleLit)) varDecl.Type = new TypeRef(doubleLit.GetType());
        }
    }
}