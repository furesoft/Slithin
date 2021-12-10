using Furesoft.Core.CodeDom.CodeDOM.Annotations;
using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Other;
using Slithin.ActionCompiler.Parsing.AST;
using Slithin.ActionCompiler.Parsing.AST.References;
using Slithin.ActionCompiler.TypeSystem;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Slithin.ActionCompiler.Compiling.Passes;

//ToDo: Change to Custom Typesystem
public class TypeResolvePass : IPass
{
    private readonly Dictionary<Type, Primitive> NetTypeMap = new()
    {
        [typeof(int)] = Primitives.Int,
        [typeof(bool)] = Primitives.Int,
        [typeof(long)] = Primitives.Long,
        [typeof(float)] = Primitives.Float,
        [typeof(double)] = Primitives.Double
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

        if (varDecl.Type is UnresolvedRef { Reference: string s })
        {
            if (TypeDescriptorFactory.IsPrimitive(s))
                varDecl.Type = new PrimitiveTypeRef(TypeDescriptorFactory.FromTypeName(s));
            else
                varDecl.AttachMessage($"Cannot find type {s}", MessageSeverity.Error, MessageSource.Resolve);
        }
        else if (varDecl.Type == null)
        {
            //literal, unresolvedRef for vardecl
            if (value is Literal lit)
            {
                Type type = null;
                if (bool.TryParse(lit.Text, out var boolLit))
                    type = boolLit.GetType();
                else if (int.TryParse(lit.Text, out var intLit))
                    type = intLit.GetType();
                else if (long.TryParse(lit.Text, out var longLit))
                    type = longLit.GetType();
                else if (float.TryParse(lit.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                             out var floatLit))
                    type = floatLit.GetType();
                else if (double.TryParse(lit.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                             out var doubleLit)) type = doubleLit.GetType();
                else
                    value.AttachMessage(
                        $"Cannot find type of literal '{lit.Text}'. Specify Type or use literal suffix.",
                        MessageSeverity.Error, MessageSource.Resolve);

                varDecl.Type = new PrimitiveTypeRef(NetTypeMap[type]);
            }
        }
    }
}