using Furesoft.Core.CodeDom.CodeDOM.Base.Interfaces;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Variables.Base;
using System.Reflection;

namespace Slithin.ActionCompiler.Parsing.AST.References;

public class VarRef : VariableRef
{
    public VarRef(IVariableDecl declaration, bool isFirstOnLine) : base(declaration, isFirstOnLine)
    {
    }

    public VarRef(MemberInfo memberInfo, bool isFirstOnLine) : base(memberInfo, isFirstOnLine)
    {
    }

    public VarRef(ParameterInfo parameterInfo, bool isFirstOnLine) : base(parameterInfo, isFirstOnLine)
    {
    }
}