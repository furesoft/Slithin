namespace Slithin.ActionCompiler.TypeSystem
{
    public static class Primitives
    {
        public static Primitive Bool = new Primitive("i8", 1);
        public static Primitive Double = new Primitive("f64", 5);
        public static Primitive Float = new Primitive("f32", 4);
        public static Primitive Int = new Primitive("i32", 2);
        public static Primitive Long = new Primitive("i64", 3);
    }
}