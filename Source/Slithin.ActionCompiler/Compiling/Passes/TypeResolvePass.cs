using Furesoft.Core.CodeDom.CodeDOM.Annotations;
using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Assignments;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Base;
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
        {
            ResolveVarType(varDecl);
        }
        else if (obj is Assignment ass)
        {
            ResolveAssignment(ass);
        }

        return obj;
    }

    private (Primitive primitive, object typedValue) ParseValueAsTypedLiteral(Expression value, Literal lit)
    {
        Type type = null;
        object typedValue = null;
        if (bool.TryParse(lit.Text, out var boolLit))
        {
            type = boolLit.GetType();
            typedValue = boolLit;
        }
        else if (int.TryParse(lit.Text, out var intLit))
        {
            type = intLit.GetType();
            typedValue = intLit;
        }
        else if (long.TryParse(lit.Text, out var longLit))
        {
            type = longLit.GetType();
            typedValue = longLit;
        }
        else if (float.TryParse(lit.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                     out var floatLit))
        {
            type = floatLit.GetType();
            typedValue = floatLit;
        }
        else if (double.TryParse(lit.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                     out var doubleLit))
        {
            type = doubleLit.GetType();
            typedValue = doubleLit;
        }
        else
        {
            value.AttachMessage(
                $"Cannot find type of literal '{lit.Text}'. Specify Type or use literal suffix.",
                MessageSeverity.Error, MessageSource.Resolve);
        }

        Primitive primitive = NetTypeMap[type];

        return (primitive, typedValue);
    }

    private void ResolveAssignment(Assignment ass)
    {
        if (ass.Right is BinaryOperator binary)
        {
            TypeBinaryLiterals(binary);
        }
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
        else if (value is Literal lit)
        {
            var typed = ParseValueAsTypedLiteral(value, lit);
            varDecl.Type ??= new PrimitiveTypeRef(typed.primitive);

            varDecl.Initialization = new TypedLiteral(typed.primitive, typed.typedValue);
        }
    }

    private Expression TypeBinaryLiterals(Expression obj)
    {
        if (obj is BinaryOperator binary)
        {
            binary.Left = TypeBinaryLiterals(binary.Left);
            binary.Right = TypeBinaryLiterals(binary.Right);

            return binary;
        }
        else if (obj is Literal lit)
        {
            var typed = ParseValueAsTypedLiteral(lit, lit);

            if (typed.primitive is not null && typed.typedValue is not null)
            {
                return new TypedLiteral(typed.primitive, typed.typedValue);
            }
        }

        return obj;
    }
}