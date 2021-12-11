using System.Collections.Generic;

namespace Slithin.ActionCompiler.TypeSystem
{
    public static class TypeDescriptorFactory
    {
        private static readonly Dictionary<string, TypeDescriptor> _primitives = new();

        static TypeDescriptorFactory()
        {
            _primitives.Add("i32", Primitives.Int);
            _primitives.Add("i64", Primitives.Long);
            _primitives.Add("f32", Primitives.Float);
            _primitives.Add("f64", Primitives.Double);
            _primitives.Add("string", Primitives.String);
        }

        public static TypeDescriptor FromTypeName(string typename)
        {
            if (_primitives.ContainsKey(typename))
            {
                return _primitives[typename];
            }
            else
            {
                return null; //Todo: implement custom types
            }
        }

        public static bool IsPrimitive(string typename)
        {
            return _primitives.ContainsKey(typename);
        }
    }
}