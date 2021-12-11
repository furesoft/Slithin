namespace Slithin.ActionCompiler.TypeSystem
{
    public class PointerTypeDescriptor : TypeDescriptor
    {
        public PointerTypeDescriptor(TypeDescriptor type) : base("Pointer<?>", Primitives.Long.Token)
        {
            PointerType = type;
        }

        public TypeDescriptor PointerType { get; set; }

        public override string ToString()
        {
            return $"Pointer<{PointerType.Name}>";
        }
    }
}