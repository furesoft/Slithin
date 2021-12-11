namespace Slithin.ActionCompiler.TypeSystem
{
    public static class Primitives
    {
        public static Primitive Double = new Primitive("f64", 4);

        public static Primitive Float = new Primitive("f32", 3);

        public static Primitive Int = new Primitive("i32", 1);

        public static Primitive Long = new Primitive("i64", 2);
        public static Primitive String = new Primitive("string", 5);

        //Token = 6 For Pointer

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