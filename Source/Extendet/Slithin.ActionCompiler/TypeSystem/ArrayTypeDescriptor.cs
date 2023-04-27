namespace Slithin.ActionCompiler.TypeSystem
{
    public class ArrayTypeDescriptor : TypeDescriptor
    {
        public ArrayTypeDescriptor(TypeDescriptor type, int[] dimensions) : base($"Array[{type.Name}]", (int)PrimitiveTypeTokens.Array)
        {
            ArrayType = type;
            Dimensions = dimensions;
        }

        public TypeDescriptor ArrayType { get; set; }
        public int[] Dimensions { get; set; }

        public override string ToString()
        {
            return $"{ArrayType.Name}[{string.Join(",", Dimensions)}]";
        }
    }
}