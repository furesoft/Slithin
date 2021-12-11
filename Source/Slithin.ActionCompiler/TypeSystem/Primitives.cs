namespace Slithin.ActionCompiler.TypeSystem
{
    public static class Primitives
    {
        public static Primitive Double = new Primitive("f64", (int)PrimitiveTypeTokens.Double);

        public static Primitive Float = new Primitive("f32", (int)PrimitiveTypeTokens.Float);

        public static Primitive Int = new Primitive("i32", (int)PrimitiveTypeTokens.Int);

        public static Primitive Long = new Primitive("i64", (int)PrimitiveTypeTokens.Long);
        public static Primitive String = new Primitive("string", (int)PrimitiveTypeTokens.String);

        static Primitives()
        {
            InitBaseOperators(Double);
            InitBaseOperators(Float);
            InitBaseOperators(Int);
            InitBaseOperators(Long);
            //ToDo: add operators for string

            InitImplicitCastInt();
        }

        public static void InitBaseOperators(Primitive primitive)
        {
            OperatorInfo operatorInfo = new(primitive.Name) { primitive.Name, primitive.Name };

            primitive.Add("+", operatorInfo);
            primitive.Add("-", operatorInfo);
            primitive.Add("*", operatorInfo);
            primitive.Add("/", operatorInfo);
        }

        private static void InitImplicitCastInt()
        {
            //int -> long, -> Float -> Double
        }
    }
}