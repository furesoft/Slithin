using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.Rendering;

namespace Slithin.ActionCompiler.TypeSystem
{
    public class TypedLiteral : Expression
    {
        public TypedLiteral(TypeDescriptor typeDescriptor, object value)
        {
            TypeDescriptor = typeDescriptor;
            Value = value;
        }

        public TypeDescriptor TypeDescriptor { get; set; }
        public object Value { get; set; }

        public override void AsTextExpression(CodeWriter writer, RenderFlags flags)
        {
            writer.Write(Value.ToString());
        }
    }
}