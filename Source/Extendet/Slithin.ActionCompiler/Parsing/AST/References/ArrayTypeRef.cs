using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Base.Interfaces;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.AnonymousMethods;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Other;
using Furesoft.Core.CodeDom.CodeDOM.Statements.Base;
using Furesoft.Core.CodeDom.Parsing;
using Furesoft.Core.CodeDom.Rendering;
using System;
using System.Linq;
using System.Reflection;

namespace Slithin.ActionCompiler.Parsing.AST.References;

public class ArrayTypeRef : TypeRefBase
{
    public ArrayTypeRef(string name, bool isFirstOnLine) : base(name, isFirstOnLine)
    {
    }

    public ArrayTypeRef(ITypeDecl typeDecl, bool isFirstOnLine) : base(typeDecl, isFirstOnLine)
    {
    }

    public ArrayTypeRef(Type memberInfo, bool isFirstOnLine) : base(memberInfo, isFirstOnLine)
    {
    }

    public ArrayTypeRef(MethodDeclBase methodDeclBase, bool isFirstOnLine) : base(methodDeclBase, isFirstOnLine)
    {
    }

    public ArrayTypeRef(AnonymousMethod anonymousMethod, bool isFirstOnLine) : base(anonymousMethod, isFirstOnLine)
    {
    }

    public ArrayTypeRef(MethodBase methodBase, bool isFirstOnLine) : base(methodBase, isFirstOnLine)
    {
    }

    public ArrayTypeRef(object obj) : base(obj)
    {
    }

    public ArrayTypeRef(Parser parser, CodeObject parent) : base(parser, parent)
    {
    }

    public UnresolvedRef Base { get; set; }
    public ChildList<Expression> Dimensions { get; set; } = new ChildList<Expression>();

    public new static void AddParsePoints()
    {
        Parser.AddParsePoint("[", 1, Parse);
    }

    public override void AsTextExpression(CodeWriter writer, RenderFlags flags)
    {
        writer.WriteIdentifier(Base._AsString, flags);

        writer.Write("[");
        writer.Write(string.Join(", ", Dimensions.Select(_ => _._AsString)));
        writer.Write("]");
    }

    public override string GetFullName()
    {
        return Base._AsString + "[" + string.Join(", ", Dimensions.Select(_ => _._AsString)) + "]";
    }

    private static CodeObject Parse(Parser parser, CodeObject parent, ParseFlags flags)
    {
        var reference = new ArrayTypeRef(parser, parent);
        var ptoken = parser.ParentStartingToken;

        reference.Base = new UnresolvedRef(ptoken.Text);

        parser.NextToken();

        reference.Dimensions = Expression.ParseList(parser, reference, "]");

        parser.NextToken();

        return reference;
    }
}