using Furesoft.Core.CodeDom.CodeDOM.Annotations;
using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Types;
using Slithin.ActionCompiler.Parsing.AST;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Slithin.ActionCompiler.Compiling.Passes;

//ToDo: Change to Custom Typesystem
public class TypeResolvePass : IPass
{
    private Dictionary<string, Type> TypeMap = new()
    {
        ["i32"] = typeof(int),
        ["bool"] = typeof(bool),
        ["i64"] = typeof(long),
        ["f32"] = typeof(float),
        ["f64"] = typeof(double),
    };

    public CodeObject Process(CodeObject obj, PassManager passManager)
    {
        if (obj is VarDecl varDecl)
            ResolveVarType(varDecl);

        return obj;
    }

    private void ResolveVarType(VarDecl varDecl)
    {
        var value = varDecl.Initialization;

        if (varDecl.Type is UnresolvedRef uref && uref.Reference is string s)
        {
            if (TypeMap.ContainsKey(s))
            {
                varDecl.Type = TypeMap[s];
            }
            else
            {
                varDecl.AttachMessage($"Cannot find type {s}", MessageSeverity.Error, MessageSource.Resolve);
            }
        }
        else
        {
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
                else
                    value.AttachMessage($"Cannot find type of literal '{lit.Text}'. Specify Type or use literal suffix.", MessageSeverity.Error, MessageSource.Resolve);
            }
        }
    }
}