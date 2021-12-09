using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Base.Interfaces;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Types;
using Furesoft.Core.CodeDom.Parsing;
using Furesoft.Core.CodeDom.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slithin.ActionCompiler.Parsing.AST.References;

public class GenericTypeRef : TypeRef
{
    public GenericTypeRef(ITypeDecl iTypeDecl) : base(iTypeDecl)
    {
    }

    public GenericTypeRef(Type type) : base(type)
    {
    }

    public GenericTypeRef(object constantValue) : base(constantValue)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, bool isFirstOnLine) : base(iTypeDecl, isFirstOnLine)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, ChildList<Expression> typeArguments) : base(iTypeDecl, typeArguments)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, List<int> arrayRanks) : base(iTypeDecl, arrayRanks)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, params Expression[] typeArguments) : base(iTypeDecl, typeArguments)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, params int[] arrayRanks) : base(iTypeDecl, arrayRanks)
    {
    }

    public GenericTypeRef(Type type, bool isFirstOnLine) : base(type, isFirstOnLine)
    {
    }

    public GenericTypeRef(Type type, ChildList<Expression> typeArguments) : base(type, typeArguments)
    {
    }

    public GenericTypeRef(Type type, List<int> arrayRanks) : base(type, arrayRanks)
    {
    }

    public GenericTypeRef(Type type, params Expression[] typeArguments) : base(type, typeArguments)
    {
    }

    public GenericTypeRef(Type type, params int[] arrayRanks) : base(type, arrayRanks)
    {
    }

    public GenericTypeRef(TypeRef typeRef, object constantValue) : base(typeRef, constantValue)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, bool isFirstOnLine, ChildList<Expression> typeArguments) : base(iTypeDecl, isFirstOnLine, typeArguments)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, ChildList<Expression> typeArguments, List<int> arrayRanks) : base(iTypeDecl, typeArguments, arrayRanks)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, bool isFirstOnLine, params Expression[] typeArguments) : base(iTypeDecl, isFirstOnLine, typeArguments)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, bool isFirstOnLine, params int[] arrayRanks) : base(iTypeDecl, isFirstOnLine, arrayRanks)
    {
    }

    public GenericTypeRef(Type type, bool isFirstOnLine, ChildList<Expression> typeArguments) : base(type, isFirstOnLine, typeArguments)
    {
    }

    public GenericTypeRef(Type type, ChildList<Expression> typeArguments, List<int> arrayRanks) : base(type, typeArguments, arrayRanks)
    {
    }

    public GenericTypeRef(Type type, bool isFirstOnLine, params Expression[] typeArguments) : base(type, isFirstOnLine, typeArguments)
    {
    }

    public GenericTypeRef(Type type, bool isFirstOnLine, params int[] arrayRanks) : base(type, isFirstOnLine, arrayRanks)
    {
    }

    public GenericTypeRef(ITypeDecl iTypeDecl, bool isFirstOnLine, ChildList<Expression> typeArguments, List<int> arrayRanks) : base(iTypeDecl, isFirstOnLine, typeArguments, arrayRanks)
    {
    }

    public GenericTypeRef(Type type, bool isFirstOnLine, ChildList<Expression> typeArguments, List<int> arrayRanks) : base(type, isFirstOnLine, typeArguments, arrayRanks)
    {
    }

    protected GenericTypeRef(Parser parser, CodeObject parent, bool isBuiltIn, ParseFlags flags) : base(parser, parent, isBuiltIn, flags)
    {
    }

    public ChildList<Expression> Arguments { get; set; } = new ChildList<Expression>();
    public UnresolvedRef Base { get; set; }

    public new static void AddParsePoints()
    {
        Parser.AddParsePoint("<", 1, Parse, typeof(VarDecl));
    }

    public override void AsTextExpression(CodeWriter writer, RenderFlags flags)
    {
        writer.WriteIdentifier(Base._AsString, flags);

        writer.Write("<");
        writer.Write(string.Join(", ", Arguments.Select(_ => _._AsString)));
        writer.Write(">");
    }

    private static CodeObject Parse(Parser parser, CodeObject parent, ParseFlags flags)
    {
        var reference = new GenericTypeRef(parser, parent, false, flags);
        var ptoken = parser.ParentStartingToken;

        reference.Base = new UnresolvedRef(ptoken.Text);
        reference.Arguments = Expression.ParseList(parser, reference, ">");

        parser.NextToken();

        return reference;
    }
}