using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Slithin.ModuleSystem.WASInterface;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem;

public static class ModuleImporter
{
    public static readonly List<Type> Types = new();

    public static void Export(Type type, dynamic instance)
    {
        foreach (var field in type.GetFields())
        {
            if (!field.IsStatic) continue;

            var fiattr = field.GetCustomAttribute<WasmImportValueAttribute>();

            if (fiattr != null)
            {
                var mem = Sg_wasm.Mem + fiattr.Offset;

                object value = null;

                if (field.FieldType.IsValueType)
                    value = Marshal.PtrToStructure(mem, field.FieldType);
                else if (field.FieldType == typeof(string))
                    value = fiattr.Length != default
                        ? Marshal.PtrToStringUTF8(mem, fiattr.Length)
                        : Marshal.PtrToStringUTF8(mem);

                field.SetValue(null, value);
            }

            var giattr = field.GetCustomAttribute<WasmImportGlobalAttribute>();

            if (giattr != null)
            {
                Type instanceType = instance.GetType();
                foreach (var prop in instanceType.GetProperties())
                    if (giattr.Name == prop.Name)
                        field.SetValue(null, prop.GetValue(instance));
            }
        }
    }

    public static void Import(Type type, ImportDictionary imports)
    {
        var attr = type.GetCustomAttribute<WasmExportAttribute>();

        Types.Add(type);

        IDictionary<string, RuntimeImport> dict = null;

        if (attr != null)
        {
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

        foreach (var field in type.GetFields())
        {
            if (!field.IsStatic) continue;
            var fattr = field.GetCustomAttribute<WasmExportValueAttribute>();

            if (fattr != null)
            {
                var value = field.GetValue(null);

                var mem = Sg_wasm.Mem + fattr.Offset;

                if (field.FieldType.IsValueType)
                {
                    Marshal.StructureToPtr(value, mem, false);
                }
                else if (value is string str)
                {
                    var utf8 = Util.ToUtf8(str);

                    Marshal.Copy(utf8, 0, mem, str.Length);
                }
            }

            var gattr = field.GetCustomAttribute<WasmExportGlobalAttribute>();
            if (gattr != null)
            {
                var value = field.GetValue(null);
                dict.Add(gattr.Name, new GlobalImport(() => (int) value, _ => field.SetValue(null, _)));
            }
        }
    }

    private static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
    {
        Func<Type[], Type> getType;
        var isAction = methodInfo.ReturnType == typeof(void);
        var types = methodInfo.GetParameters().Select(p => p.ParameterType);

        if (isAction)
        {
            getType = Expression.GetActionType;
        }
        else
        {
            getType = Expression.GetFuncType;
            types = types.Concat(new[] {methodInfo.ReturnType});
        }

        return methodInfo.IsStatic
            ? Delegate.CreateDelegate(getType(types.ToArray()), methodInfo)
            : Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
    }

    private static void ImportFunctions(Type type, IDictionary<string, RuntimeImport> dict)
    {
        foreach (var method in type.GetMethods())
        {
            var attr = method.GetCustomAttribute<WasmExportAttribute>();

            if (attr == null || !method.IsStatic) continue;

            var del = CreateDelegate(method, null);

            dict.Add(attr.Name, new FunctionImport(del));
        }
    }
}