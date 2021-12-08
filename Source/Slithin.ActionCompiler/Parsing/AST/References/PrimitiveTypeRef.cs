using System.Reflection;
using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Base.Interfaces;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.AnonymousMethods;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Base;
using Furesoft.Core.CodeDom.Parsing;

namespace Slithin.ActionCompiler.Parsing.AST.References;

public class PrimitiveTypeRef : SymbolicRef
{
    public PrimitiveTypeRef(string name, bool isFirstOnLine) : base(name, isFirstOnLine)
    {
    }

    public PrimitiveTypeRef(INamedCodeObject namedCodeObject, bool isFirstOnLine) : base(namedCodeObject, isFirstOnLine)
    {
    }

    public PrimitiveTypeRef(AnonymousMethod anonymousMethod, bool isFirstOnLine) : base(anonymousMethod, isFirstOnLine)
    {
    }

    public PrimitiveTypeRef(MemberInfo memberInfo, bool isFirstOnLine) : base(memberInfo, isFirstOnLine)
    {
    }

    public PrimitiveTypeRef(ParameterInfo parameterInfo, bool isFirstOnLine) : base(parameterInfo, isFirstOnLine)
    {
    }

    public PrimitiveTypeRef(object obj) : base(obj)
    {
    }

    public PrimitiveTypeRef(Parser parser, CodeObject parent) : base(parser, parent)
    {
    }
}