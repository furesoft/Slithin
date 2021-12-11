namespace Slithin.ActionCompiler.TypeSystem
{
    public class PointerTypeDescriptor : TypeDescriptor
    {
        public PointerTypeDescriptor(TypeDescriptor type) : base("Pointer<?>", 6)
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