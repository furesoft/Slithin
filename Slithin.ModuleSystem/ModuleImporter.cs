using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem
{
    public static class ModuleImporter
    {
        public static void Import(Type type, ImportDictionary imports)
        {
            var attr = type.GetCustomAttribute<WasmExportAttribute>();

            if (attr != null)
            {
                IDictionary<string, RuntimeImport> dict;
                if (imports.ContainsKey(attr.Name))
                {
                    dict = imports[attr.Name];
                }
                else
                {
                    dict = new Dictionary<string, RuntimeImport>();
                    imports.Add(attr.Name, dict);
                }

                ImportFunctions(type, dict);
            }
        }

        private static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType.Equals((typeof(void)));
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic)
            {
                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
            }

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }

        private static void ImportFunctions(Type type, IDictionary<string, RuntimeImport> dict)
        {
            foreach (var method in type.GetMethods())
            {
                var attr = method.GetCustomAttribute<WasmExportAttribute>();

                if (attr != null && method.IsStatic)
                {
                    var del = CreateDelegate(method, null);

                    dict.Add(attr.Name, new FunctionImport(del));
                }
            }
        }
    }
}